using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Drawing;
using Duality.Properties;
using DynamicLighting.Properties;

//	Note:
//
//	Rewrite dynamic lighting to not reverse-engineer a vertices world position in the shader code.
//	-->	Instead, add an additional worldPosition attribute to the vertex format.
//	-->	This is the only way to allow flexible lighting when using different perspective modes.

namespace DynamicLighting
{
	/// <summary>
	/// A source of light. Supported light types are directional, ambient, point and spot.
	/// To create directional or ambient lights, add this Component to a GameObject without Transform.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(DynLightResNames.IconResourceLight)]
    public class Light : Component
    {
		private class DeviceLightInfo
		{
			public	List<Light>		PriorizedLights;
			public	int				FrameId;
		}
		private	static	Dictionary<IDrawDevice,DeviceLightInfo>	deviceInfo	= new Dictionary<IDrawDevice,DeviceLightInfo>();

		public	const	int	MaxVisible	= 8;

		private	Vector3		dir					= Vector3.UnitZ;
		private	ColorRgba	color				= ColorRgba.White;
		private	ColorRgba	ambientColor		= ColorRgba.White;
		private	float		intensity			= 1.0f;
		private	float		ambientIntensity	= 0.0f;
		private	float		range				= 1500.0f;
		private	float		spotFocus			= 0.0f;

		/// <summary>
		/// [GET / SET] The direction this Light points to. Used in spot and directional light. Set this to zero
		/// in order to create a point light.
		/// </summary>
		public Vector3 Direction
		{
			get { return this.dir; }
			set
			{
				this.dir = value;
				if (this.dir != Vector3.Zero) this.dir.Normalize();
			}
		}
		/// <summary>
		/// [GET / SET] The Lights main color.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ColorRgba Color
		{
			get { return this.color; }
			set
			{
				ColorHsva hsva = value.ToHsva();
				if (hsva.V != 1.0f || hsva.A != 1.0f)
				{
					this.intensity = hsva.V * hsva.A;
					hsva.V = 1.0f;
					hsva.A = 1.0f;
				}
				this.color = hsva.ToRgba();
			}
		}
		/// <summary>
		/// [GET / SET] The Lights intensity.
		/// </summary>
		public float Intensity
		{
			get { return this.intensity; }
			set { this.intensity = value; }
		}
		/// <summary>
		/// [GET / SET] The Lights ambient color value. Ambient light is used as base value in directional lighting.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ColorRgba AmbientColor
		{
			get { return this.ambientColor; }
			set
			{
				ColorHsva hsva = value.ToHsva();
				if (hsva.V != 1.0f || hsva.A != 1.0f)
				{
					this.ambientIntensity = hsva.V * hsva.A;
					hsva.V = 1.0f;
					hsva.A = 1.0f;
				}
				this.ambientColor = hsva.ToRgba();
			}
		}
		/// <summary>
		/// [GET / SET] The Lights ambient intensity. Ambient light is used as base value in directional lighting.
		/// </summary>
		public float AmbientIntensity
		{
			get { return this.ambientIntensity; }
			set { this.ambientIntensity = value; }
		}
		/// <summary>
		/// [GET / SET] The Lights range. Only applies to point and spot lights.
		/// </summary>
		[EditorHintIncrement(10.0f)]
		public float Range
		{
			get { return this.range; }
			set { this.range = value; }
		}
		/// <summary>
		/// [GET / SET] The Lights spot focus. The higher this value is, the smaller the spot radius.
		/// </summary>
		[EditorHintRange(1.0f, 100.0f)]
		public float SpotFocus
		{
			get { return this.spotFocus; }
			set { this.spotFocus = value; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsDirectional
		{
			get { return this.GameObj == null || this.GameObj.Transform == null; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsSpot
		{
			get { return !this.IsDirectional && this.dir != Vector3.Zero; }
		}

		public bool IsVisibleTo(IDrawDevice device)
		{
			if (this.IsDirectional) return true;
			if (this.range <= 0.0f) return false;
			if (this.intensity <= 0.0f) return false;

			float uniformScale = this.GameObj.Transform.Scale;
			if (uniformScale <= 0.0f) return false;

			Vector3 pos = this.GameObj.Transform.Pos;
			if (device.IsCoordInView(pos, this.range * uniformScale)) return true;
			if (device.IsCoordInView(pos - Vector3.UnitZ * this.range * 0.5f * uniformScale, this.range * uniformScale)) return true;
			if (device.IsCoordInView(pos + Vector3.UnitZ * this.range * uniformScale, this.range * uniformScale)) return true;
			return false;
		}
		public int CalcPriority(IDrawDevice device)
		{
			if (!this.IsDirectional)
			{
				float uniformScale = this.GameObj.Transform.Scale;
				Vector3 pos = this.GameObj.Transform.Pos;
				float scale = 1.0f;
				device.PreprocessCoords(ref pos, ref scale);

				float planarDist = (this.GameObj.Transform.Pos.Xy - device.RefCoord.Xy).Length;

				float rangeFactor = 1.0f / (this.range * uniformScale);
				float distFactor = (MathF.Min(scale, 1.0f) * planarDist);

				float spotFactor;
				if (this.dir != Vector3.Zero)
					spotFactor = 0.5f * (1.0f + Vector3.Dot((device.RefCoord - this.GameObj.Transform.Pos).Normalized, this.dir));
				else
					spotFactor = 1.0f;

				return MathF.RoundToInt(1000000.0f * spotFactor * distFactor * MathF.Pow(rangeFactor, 1.5f) * MathF.Pow(1.0f / this.intensity, 0.5f));
			}
			else
			{
				return MathF.RoundToInt(100.0f * MathF.Pow(1.0f / this.intensity, 0.5f));
			}
		}

		public static void UpdateLighting(IDrawDevice device)
		{
			// Only update lighting info once per frame and device
			DeviceLightInfo info;
			if (deviceInfo.TryGetValue(device, out info) && info != null && info.FrameId == Time.FrameCount)
				return;

			if (info == null)
			{
				info = new DeviceLightInfo();
				deviceInfo[device] = info;
			}
			info.FrameId = Time.FrameCount;
			info.PriorizedLights = Scene.Current.FindComponents<Light>().Where(l => l.Active).Where(l => l.IsVisibleTo(device)).ToList();
			info.PriorizedLights.StableSort((Light a, Light b) => a.CalcPriority(device) - b.CalcPriority(device));

			// Prepare shader dara
			Vector4[] _lightPos = new Vector4[MaxVisible];
			Vector4[] _lightDir = new Vector4[MaxVisible];
			Vector3[] _lightColor = new Vector3[MaxVisible];
			int _lightCount = MathF.Min(MaxVisible, info.PriorizedLights.Count);

			int i = 0;
			foreach (Light light in info.PriorizedLights)
			{
				if (light.Disposed) continue;

				Vector3 dir;
				Vector3 pos;
				float uniformScale;
				bool directional = light.IsDirectional;
				if (directional)
				{
					dir = light.dir;
					pos = Vector3.Zero;
					uniformScale = 1.0f;
				}
				else
				{
					dir = light.dir;
					pos = light.GameObj.Transform.Pos;
					uniformScale = light.GameObj.Transform.Scale;

					MathF.TransformCoord(ref dir.X, ref dir.Y, light.GameObj.Transform.Angle);
				}

				if (directional)
				{
					_lightPos[i].X = (float)light.ambientColor.R * light.ambientIntensity / 255.0f;
					_lightPos[i].Y = (float)light.ambientColor.G * light.ambientIntensity / 255.0f;
					_lightPos[i].Z = (float)light.ambientColor.B * light.ambientIntensity / 255.0f;
					_lightPos[i].W = 0.0f;
				}
				else
				{
					_lightPos[i].X = pos.X;
					_lightPos[i].Y = pos.Y;
					_lightPos[i].Z = pos.Z;
					_lightPos[i].W = light.range * uniformScale;
				}

				_lightDir[i].X = dir.X;
				_lightDir[i].Y = dir.Y;
				_lightDir[i].Z = dir.Z;
				_lightDir[i].W = dir == Vector3.Zero ? 0.0f : MathF.Max(light.spotFocus, 1.0f);

				_lightColor[i].X = (float)light.color.R * light.intensity / 255.0f;
				_lightColor[i].Y = (float)light.color.G * light.intensity / 255.0f;
				_lightColor[i].Z = (float)light.color.B * light.intensity / 255.0f;

				i++;
				if (i >= _lightCount) break;
			}
			if (i + 1 < _lightCount) _lightCount = i + 1;

			device.ShaderParameters.Set("_lightCount", _lightCount);
			device.ShaderParameters.Set("_lightPos", _lightPos);
			device.ShaderParameters.Set("_lightDir", _lightDir);
			device.ShaderParameters.Set("_lightColor", _lightColor);
		}
		public static void GetLightAtWorldPos(Vector3 worldPos, out Vector4 lightColor, float translucency = 0.0f)
		{
			lightColor = Vector4.UnitW;

			IEnumerable<Light> lights = Scene.Current.FindComponents<Light>();
			foreach (Light light in lights)
			{
				if (light.Disposed) continue;
				if (!light.Active) continue;
				if (light.IsDirectional)
				{
					float translucencyFactor = Vector3.Dot(light.dir, Vector3.UnitZ);
					translucencyFactor = translucencyFactor + 0.5f + 1.5f * translucency;
					translucencyFactor = MathF.Sign(translucencyFactor) * MathF.Pow(MathF.Abs(translucencyFactor), 0.5f);
					translucencyFactor = MathF.Clamp(translucencyFactor, 0.0f, 1.0f);

					Vector3 color = new Vector3(
						(float)light.color.R * light.intensity / 255.0f,
						(float)light.color.G * light.intensity / 255.0f,
						(float)light.color.B * light.intensity / 255.0f);
					Vector3.Multiply(ref color, translucencyFactor, out color);

					lightColor += new Vector4(color);
				}
				else
				{
					Vector3 lightPos = light.GameObj.Transform.Pos;
					Vector3 lightVec = lightPos - worldPos;
					float dist = lightVec.Length;
					Vector3 lightVecNorm = lightVec / dist;
					float spotExp = light.dir == Vector3.Zero ? 0.0f : MathF.Max(light.spotFocus, 1.0f);
					
					Vector3 lightDir = light.dir;
					float uniformScale = light.GameObj.Transform.Scale;

					MathF.TransformCoord(ref lightDir.X, ref lightDir.Y, light.GameObj.Transform.Angle);

					float attenFactor = 1.0f - MathF.Min(dist / (light.range * uniformScale), 1.0f);
					float spotFactor = MathF.Pow(MathF.Max(Vector3.Dot(lightVecNorm, -lightDir), 0.000001f), spotExp);
					float translucencyFactor = Vector3.Dot(lightVecNorm, -Vector3.UnitZ);
					translucencyFactor = translucencyFactor + 0.5f + 1.5f * translucency;
					translucencyFactor = MathF.Sign(translucencyFactor) * MathF.Pow(MathF.Abs(translucencyFactor), 0.5f);
					translucencyFactor = MathF.Clamp(translucencyFactor, 0.0f, 1.0f);
					
					Vector3 color = new Vector3(
						(float)light.color.R * light.intensity / 255.0f,
						(float)light.color.G * light.intensity / 255.0f,
						(float)light.color.B * light.intensity / 255.0f);
					Vector3.Multiply(ref color, attenFactor * spotFactor * translucencyFactor, out color);

					lightColor += new Vector4(color);
				}
			}
		}
	}
}
