using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Editor;
using Duality.Resources;
using Duality.Serialization;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes a set of shader parameters independently from any specific shader.
	/// It's a CPU side key-value store for values that can be applied to a shader
	/// program by the <see cref="Duality.Backend.IGraphicsBackend"/>.
	/// </summary>
	public class ShaderParameters : IEquatable<ShaderParameters>, ISerializeExplicit
	{
		private Dictionary<string,ContentRef<Texture>> textures = null;
		private Dictionary<string,float[]>             uniforms = null;
		private long                                   hash     = 0;
		
		
		/// <summary>
		/// [GET / SET] Shortcut for accessing the <see cref="ShaderFieldInfo.DefaultNameMainTex"/> texture variable.
		/// </summary>
		public ContentRef<Texture> MainTexture
		{
			get { return this.Get<ContentRef<Texture>>(ShaderFieldInfo.DefaultNameMainTex); }
			set { this.Set(ShaderFieldInfo.DefaultNameMainTex, value); }
		}


		public ShaderParameters()
		{
			this.uniforms = new Dictionary<string,float[]>();
			this.textures = new Dictionary<string,ContentRef<Texture>>();
			this.UpdateHash();
		}
		public ShaderParameters(ShaderParameters other)
		{
			this.textures = new Dictionary<string,ContentRef<Texture>>(other.textures);
			this.uniforms = new Dictionary<string,float[]>(other.uniforms.Count);
			foreach (var pair in other.uniforms)
			{
				this.uniforms[pair.Key] = (float[])pair.Value.Clone();
			}
			this.hash = other.hash;
		}

		/// <summary>
		/// Removes all variables and values from the <see cref="ShaderParameters"/> instance.
		/// </summary>
		public void Clear()
		{
			this.textures.Clear();
			this.uniforms.Clear();
			this.UpdateHash();
		}

		/// <summary>
		/// Assigns an array of values to the specified variable. All values may be converted into
		/// a shared internal format.
		/// 
		/// Supported base types are <see cref="Single"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, 
		/// <see cref="Vector4"/>, <see cref="Matrix3"/>, <see cref="Matrix4"/>, <see cref="Int32"/>,
		/// <see cref="Point2"/>, <see cref="Boolean"/> and <see cref="ContentRef<Texture>"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetArray<T>(string name, params T[] value) where T : struct
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("The parameter name cannot be null or empty.", "name");
			if (value == null) throw new ArgumentNullException("value");
			if (value.Length == 0) throw new ArgumentException("At least one parameter value needs to be specified.", "value");

			float[] rawData;
			if (typeof(T) == typeof(float))
			{
				float[] typedValue = (float[])(object)value;
				this.EnsureUniformData(name, value.Length * 1, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i] = typedValue[i];
				}
			}
			else if (typeof(T) == typeof(Vector2))
			{
				Vector2[] typedValue = (Vector2[])(object)value;
				this.EnsureUniformData(name, value.Length * 2, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 2 + 0] = typedValue[i].X;
					rawData[i * 2 + 1] = typedValue[i].Y;
				}
			}
			else if (typeof(T) == typeof(Vector3))
			{
				Vector3[] typedValue = (Vector3[])(object)value;
				this.EnsureUniformData(name, value.Length * 3, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 3 + 0] = typedValue[i].X;
					rawData[i * 3 + 1] = typedValue[i].Y;
					rawData[i * 3 + 2] = typedValue[i].Z;
				}
			}
			else if (typeof(T) == typeof(Vector4))
			{
				Vector4[] typedValue = (Vector4[])(object)value;
				this.EnsureUniformData(name, value.Length * 4, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 4 + 0] = typedValue[i].X;
					rawData[i * 4 + 1] = typedValue[i].Y;
					rawData[i * 4 + 2] = typedValue[i].Z;
					rawData[i * 4 + 3] = typedValue[i].W;
				}
			}
			else if (typeof(T) == typeof(Matrix3))
			{
				Matrix3[] typedValue = (Matrix3[])(object)value;
				this.EnsureUniformData(name, value.Length * 9, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 9 + 0] = typedValue[i].Row0.X;
					rawData[i * 9 + 1] = typedValue[i].Row0.Y;
					rawData[i * 9 + 2] = typedValue[i].Row0.Z;
					rawData[i * 9 + 3] = typedValue[i].Row1.X;
					rawData[i * 9 + 4] = typedValue[i].Row1.Y;
					rawData[i * 9 + 5] = typedValue[i].Row1.Z;
					rawData[i * 9 + 6] = typedValue[i].Row2.X;
					rawData[i * 9 + 7] = typedValue[i].Row2.Y;
					rawData[i * 9 + 8] = typedValue[i].Row2.Z;
				}
			}
			else if (typeof(T) == typeof(Matrix4))
			{
				Matrix4[] typedValue = (Matrix4[])(object)value;
				this.EnsureUniformData(name, value.Length * 16, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 16 +  0] = typedValue[i].Row0.X;
					rawData[i * 16 +  1] = typedValue[i].Row0.Y;
					rawData[i * 16 +  2] = typedValue[i].Row0.Z;
					rawData[i * 16 +  3] = typedValue[i].Row0.W;
					rawData[i * 16 +  4] = typedValue[i].Row1.X;
					rawData[i * 16 +  5] = typedValue[i].Row1.Y;
					rawData[i * 16 +  6] = typedValue[i].Row1.Z;
					rawData[i * 16 +  7] = typedValue[i].Row1.W;
					rawData[i * 16 +  8] = typedValue[i].Row2.X;
					rawData[i * 16 +  9] = typedValue[i].Row2.Y;
					rawData[i * 16 + 10] = typedValue[i].Row2.Z;
					rawData[i * 16 + 11] = typedValue[i].Row2.W;
					rawData[i * 16 + 12] = typedValue[i].Row3.X;
					rawData[i * 16 + 13] = typedValue[i].Row3.Y;
					rawData[i * 16 + 14] = typedValue[i].Row3.Z;
					rawData[i * 16 + 15] = typedValue[i].Row3.W;
				}
			}
			else if (typeof(T) == typeof(int))
			{
				int[] typedValue = (int[])(object)value;
				this.EnsureUniformData(name, value.Length * 1, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i] = typedValue[i];
				}
			}
			else if (typeof(T) == typeof(Point2))
			{
				Point2[] typedValue = (Point2[])(object)value;
				this.EnsureUniformData(name, value.Length * 2, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i * 2 + 0] = typedValue[i].X;
					rawData[i * 2 + 1] = typedValue[i].Y;
				}
			}
			else if (typeof(T) == typeof(bool))
			{
				bool[] typedValue = (bool[])(object)value;
				this.EnsureUniformData(name, value.Length * 1, out rawData);
				for (int i = 0; i < value.Length; i++)
				{
					rawData[i] = typedValue[i] ? 1.0f : 0.0f;
				}
			}
			else if (typeof(T) == typeof(ContentRef<Texture>))
			{
				if (value.Length > 1) throw new ArgumentException("Can't assign more than one texture to a single shader parameter.", "value");
				this.textures[name] = (ContentRef<Texture>)(object)value[0];
				this.uniforms.Remove(name);
			}
			else
			{
				throw new NotSupportedException("Setting shader parameters to values of this type is not supported.");
			}

			this.UpdateHash();
		}
		/// <summary>
		/// Retrieves an array of values from the specified variable. If the internally 
		/// stored type does not match the specified type, it will be converted before returning.
		/// 
		/// Supported base types are <see cref="Single"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, 
		/// <see cref="Vector4"/>, <see cref="Matrix3"/>, <see cref="Matrix4"/>, <see cref="Int32"/>,
		/// <see cref="Point2"/>, <see cref="Boolean"/> and <see cref="ContentRef<Texture>"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T[] GetArray<T>(string name) where T : struct
		{
			if (string.IsNullOrEmpty(name)) return null;

			if (typeof(T) == typeof(ContentRef<Texture>))
			{
				ContentRef<Texture> tex;
				if (!this.textures.TryGetValue(name, out tex))
					return null;
				else
					return (T[])(object)new ContentRef<Texture>[] { tex };
			}
			
			float[] rawData;
			if (!this.uniforms.TryGetValue(name, out rawData)) return null;

			if (typeof(T) == typeof(float))
			{
				if (rawData.Length < 1) return null;
				float[] result = new float[rawData.Length / 1];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = rawData[i];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Vector2))
			{
				if (rawData.Length < 2) return null;
				Vector2[] result = new Vector2[rawData.Length / 2];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].X = rawData[i * 2 + 0];
					result[i].Y = rawData[i * 2 + 1];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Vector3))
			{
				if (rawData.Length < 3) return null;
				Vector3[] result = new Vector3[rawData.Length / 3];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].X = rawData[i * 3 + 0];
					result[i].Y = rawData[i * 3 + 1];
					result[i].Z = rawData[i * 3 + 2];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Vector4))
			{
				if (rawData.Length < 4) return null;
				Vector4[] result = new Vector4[rawData.Length / 4];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].X = rawData[i * 4 + 0];
					result[i].Y = rawData[i * 4 + 1];
					result[i].Z = rawData[i * 4 + 2];
					result[i].W = rawData[i * 4 + 3];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Matrix3))
			{
				if (rawData.Length < 9) return null;
				Matrix3[] result = new Matrix3[rawData.Length / 9];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].Row0.X = rawData[i * 9 + 0];
					result[i].Row0.Y = rawData[i * 9 + 1];
					result[i].Row0.Z = rawData[i * 9 + 2];
					result[i].Row1.X = rawData[i * 9 + 3];
					result[i].Row1.Y = rawData[i * 9 + 4];
					result[i].Row1.Z = rawData[i * 9 + 5];
					result[i].Row2.X = rawData[i * 9 + 6];
					result[i].Row2.Y = rawData[i * 9 + 7];
					result[i].Row2.Z = rawData[i * 9 + 8];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Matrix4))
			{
				if (rawData.Length < 16) return null;
				Matrix4[] result = new Matrix4[rawData.Length / 16];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].Row0.X = rawData[i * 16 +  0];
					result[i].Row0.Y = rawData[i * 16 +  1];
					result[i].Row0.Z = rawData[i * 16 +  2];
					result[i].Row0.W = rawData[i * 16 +  3];
					result[i].Row1.X = rawData[i * 16 +  4];
					result[i].Row1.Y = rawData[i * 16 +  5];
					result[i].Row1.Z = rawData[i * 16 +  6];
					result[i].Row1.W = rawData[i * 16 +  7];
					result[i].Row2.X = rawData[i * 16 +  8];
					result[i].Row2.Y = rawData[i * 16 +  9];
					result[i].Row2.Z = rawData[i * 16 + 10];
					result[i].Row2.W = rawData[i * 16 + 11];
					result[i].Row3.X = rawData[i * 16 + 12];
					result[i].Row3.Y = rawData[i * 16 + 13];
					result[i].Row3.Z = rawData[i * 16 + 14];
					result[i].Row3.W = rawData[i * 16 + 15];
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(int))
			{
				if (rawData.Length < 1) return null;
				int[] result = new int[rawData.Length / 1];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = MathF.RoundToInt(rawData[i]);
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(Point2))
			{
				if (rawData.Length < 2) return null;
				Point2[] result = new Point2[rawData.Length / 2];
				for (int i = 0; i < result.Length; i++)
				{
					result[i].X = MathF.RoundToInt(rawData[i * 2 + 0]);
					result[i].Y = MathF.RoundToInt(rawData[i * 2 + 1]);
				}
				return (T[])(object)result;
			}
			else if (typeof(T) == typeof(bool))
			{
				if (rawData.Length < 1) return null;
				bool[] result = new bool[rawData.Length / 1];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = rawData[i] != 0.0f;
				}
				return (T[])(object)result;
			}
			else
			{
				throw new NotSupportedException("Getting shader parameters as values of this type is not supported.");
			}
		}

		/// <summary>
		/// Assigns a values to the specified variable. All values may be converted into
		/// a shared internal format.
		/// 
		/// Supported base types are <see cref="Single"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, 
		/// <see cref="Vector4"/>, <see cref="Matrix3"/>, <see cref="Matrix4"/>, <see cref="Int32"/>,
		/// <see cref="Point2"/>, <see cref="Boolean"/> and <see cref="ContentRef<Texture>"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set<T>(string name, T value) where T : struct
		{
			this.SetArray<T>(name, value);
		}
		/// <summary>
		/// Retrieves a values from the specified variable. If the internally 
		/// stored type does not match the specified type, it will be converted before returning.
		/// 
		/// Supported base types are <see cref="Single"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, 
		/// <see cref="Vector4"/>, <see cref="Matrix3"/>, <see cref="Matrix4"/>, <see cref="Int32"/>,
		/// <see cref="Point2"/>, <see cref="Boolean"/> and <see cref="ContentRef<Texture>"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T Get<T>(string name) where T : struct
		{
			T[] array = this.GetArray<T>(name);
			if (array == null || array.Length == 0)
				return default(T);
			else
				return array[0];
		}

		/// <summary>
		/// Retrieves the internal representation of the specified variables numeric value.
		/// The returned array should be treated as read-only.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float[] GetInternalValue(string name)
		{
			float[] value;
			if (this.uniforms.TryGetValue(name, out value))
				return value;
			else
				return null;
		}
		/// <summary>
		/// Retrieves the internal representation of the specified variables texture value.
		/// The returned value should be treated as read-only.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ContentRef<Texture> GetInternalTexture(string name)
		{
			ContentRef<Texture> value;
			if (this.textures.TryGetValue(name, out value))
				return value;
			else
				return null;
		}

		private void EnsureUniformData(string name, int size, out float[] data)
		{
			if (!this.uniforms.TryGetValue(name, out data) || data.Length != size)
			{
				data = new float[size];
				this.uniforms[name] = data;
				this.textures.Remove(name);
			}
		}

		private void UpdateHash()
		{
			this.hash = 17L;
			unchecked
			{
				// Note: The order of values in a dictionary is non-deterministic,
				// thus our hash algorithm must not depend on enumeration order.

				if (this.textures.Count > 0)
				{
					foreach (var pair in this.textures)
					{
						this.hash ^= 19L * 
							(long)pair.Key.GetHashCode() * 
							(long)pair.Value.GetHashCode();
					}
				}

				if (this.uniforms.Count > 0)
				{
					foreach (var pair in this.uniforms)
					{
						long arrayHash = 23L;
						for (int i = 0; i < pair.Value.Length; i++)
						{
							arrayHash = arrayHash * 29L + (long)pair.Value[i].GetHashCode();
						}
						this.hash ^= 31L * 
							(long)pair.Key.GetHashCode() * 
							(long)arrayHash;
					}
				}
			}
		}
		public override int GetHashCode()
		{
			unchecked
			{
				return 
					(int)((ulong)this.hash) ^ 
					(int)((ulong)this.hash >> 32);
			}
		}
		public override bool Equals(object obj)
		{
			ShaderParameters other = obj as ShaderParameters;
			if (other != null)
				return this.Equals(other);
			else
				return false;
		}
		public bool Equals(ShaderParameters other)
		{
			// Quick equality heuristic for perf reasons by comparing hashes only.
			// This will fail on hash collisions. However, given that the number
			// of different materials at any time tends to be low for perf reasons,
			// collisions should be unlikely enough for this optimization to hold.
			return this.hash == other.hash;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			
			ContentRef<Texture> mainTex = this.MainTexture;
			if (mainTex != null)
			{
				builder.Append(ShaderFieldInfo.DefaultNameMainTex);
				builder.Append(" \"");
				builder.Append(mainTex.Name);
				builder.Append('"');

				if (this.textures.Count > 1)
				{
					builder.Append(", +");
					builder.Append(this.textures.Count - 1);
					builder.Append(" textures");
				}
			}
			else
			{
				builder.Append(this.textures.Count);
				builder.Append(" textures");
			}

			if (builder.Length != 0) builder.Append(", ");
			builder.Append(this.uniforms.Count);
			builder.Append(" uniforms");

			return builder.ToString();
		}

		void ISerializeExplicit.WriteData(IDataWriter writer)
		{
			writer.WriteValue("textures", this.textures);
			writer.WriteValue("uniforms", this.uniforms);
		}
		void ISerializeExplicit.ReadData(IDataReader reader)
		{
			reader.ReadValue("textures", out this.textures);
			reader.ReadValue("uniforms", out this.uniforms);

			// Retrieve references to all textures, to the ContentRefs
			// we store don't have to do a lookup on access.
			foreach (var texPair in this.textures.ToList())
			{
				ContentRef<Texture> texRef = texPair.Value;
				texRef.MakeAvailable();
				this.textures[texPair.Key] = texRef;
			}

			this.UpdateHash();
		}
	}
}