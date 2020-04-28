using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

using Duality.Resources;
using Duality.Drawing;
using Duality.Components.Diagnostics;

namespace Duality
{
	/// <summary>
	/// A <see cref="VisualLog"/> provides functionality to log spatial and other visual information conveniently, which is displayed in
	/// real-time. Each VisualLog can be set up differently and corresponds to a distinct visual layer.
	/// All submitted calls are guaranteed to be displayed visually within the same frame they were submitted in.
	/// </summary>
	public sealed class VisualLog
	{
		private	string					name			= "VisualLog";
		private	CustomVisualLogInfo		customInfo		= null;
		private	bool					visible			= true;
		private	ColorRgba				baseColor		= ColorRgba.White.WithAlpha(0.5f);
		private	VisibilityFlag			visibilityGroup	= VisibilityFlag.AllGroups;
		private	List<VisualLogEntry>	entries			= new List<VisualLogEntry>();

		/// <summary>
		/// [GET] The VisualLog's name
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// [GET] An optional <see cref="CustomVisualLogInfo"/> that was passed to the log on creation.
		/// </summary>
		public CustomVisualLogInfo CustomInfo
		{
			get { return this.customInfo; }
		}
		/// <summary>
		/// [GET / SET] Whether or not this log will be displayed. It's basically an on / off switch.
		/// </summary>
		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}
		/// <summary>
		/// [GET / SET] The logs base color, which is used for coloring all the displayed log entries.
		/// </summary>
		public ColorRgba BaseColor
		{
			get { return this.baseColor; }
			set { this.baseColor = value; }
		}
		/// <summary>
		/// [GET / SET] Similar to a <see cref="Duality.Components.Renderer">renderers</see> visibility group,
		/// this property can be used to make the log visible to only certain Cameras.
		/// </summary>
		public VisibilityFlag VisibilityGroup
		{
			get { return this.visibilityGroup; }
			set { this.visibilityGroup = value; }
		}
		/// <summary>
		/// [GET] Enumerates all log entries that have been added to this log and are currently active.
		/// </summary>
		public IReadOnlyList<VisualLogEntry> Entries
		{
			get { return this.entries; }
		}


		public VisualLog(string name, CustomVisualLogInfo customInfo = null)
		{
			this.name = name;
			this.customInfo = customInfo;
		}

		/// <summary>
		/// Draws the specified log entry. Use this method for custom <see cref="VisualLogEntry"/> class implementations.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entry"></param>
		public void Draw<T>(T entry) where T : VisualLogEntry
		{
			if (!this.visible) return;
			if (!this.entries.Contains(entry)) this.entries.Add(entry);
		}

		/// <summary>
		/// Draws a point in screen space.
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		public VisualLogPointEntry DrawPoint(float screenX, float screenY)
		{
			VisualLogPointEntry entry = new VisualLogPointEntry();
			entry.Pos = new Vector3(screenX, screenY, 0.0f);
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a point in screen space.
		/// </summary>
		/// <param name="screenPos"></param>
		public VisualLogPointEntry DrawPoint(Vector2 screenPos)
		{
			return this.DrawPoint(screenPos.X, screenPos.Y);
		}
		/// <summary>
		/// Draws a point in world space. A point has no physical size and is displayed in the same size
		/// regardless of perspective settings or distance to the Camera.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="worldZ"></param>
		public VisualLogPointEntry DrawPoint(float worldX, float worldY, float worldZ)
		{
			VisualLogPointEntry entry = new VisualLogPointEntry();
			entry.Pos = new Vector3(worldX, worldY, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a point in world space. A point has no physical size and is displayed in the same size
		/// regardless of perspective settings or distance to the Camera.
		/// </summary>
		/// <param name="worldPos"></param>
		public VisualLogPointEntry DrawPoint(Vector3 worldPos)
		{
			return this.DrawPoint(worldPos.X, worldPos.Y, worldPos.Z);
		}
		/// <summary>
		/// Draws a circle in screen space.
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		/// <param name="radius"></param>
		public VisualLogCircleEntry DrawCircle(float screenX, float screenY, float radius)
		{
			VisualLogCircleEntry entry = new VisualLogCircleEntry();
			entry.Pos = new Vector3(screenX, screenY, 0.0f);
			entry.Radius = radius;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a circle in screen space.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <param name="radius"></param>
		public VisualLogCircleEntry DrawCircle(Vector2 screenPos, float radius)
		{
			return this.DrawCircle(screenPos.X, screenPos.Y, radius);
		}
		/// <summary>
		/// Draws a circle in world space.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="worldZ"></param>
		/// <param name="radius"></param>
		public VisualLogCircleEntry DrawCircle(float worldX, float worldY, float worldZ, float radius)
		{
			VisualLogCircleEntry entry = new VisualLogCircleEntry();
			entry.Pos = new Vector3(worldX, worldY, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			entry.Radius = radius;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a circle in world space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="radius"></param>
		public VisualLogCircleEntry DrawCircle(Vector3 worldPos, float radius)
		{
			return this.DrawCircle(worldPos.X, worldPos.Y, worldPos.Z, radius);
		}
		/// <summary>
		/// Draws a vector in screen space. A vector can be thought of as an arrow pointing in a certain direction.
		/// It is useful for displaying velocities or directions.
		/// </summary>
		/// <param name="screenX">The vectors screen origin.</param>
		/// <param name="screenY">The vectors screen origin.</param>
		/// <param name="vectorX">The vector to display.</param>
		/// <param name="vectorY">The vector to display.</param>
		public VisualLogVectorEntry DrawVector(float screenX, float screenY, float vectorX, float vectorY)
		{
			VisualLogVectorEntry entry = new VisualLogVectorEntry();
			entry.Origin = new Vector3(screenX, screenY, 0.0f);
			entry.Vector = new Vector2(vectorX, vectorY);
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a vector in screen space. A vector can be thought of as an arrow pointing in a certain direction.
		/// It is useful for displaying velocities or directions.
		/// </summary>
		/// <param name="screenPos">The vectors screen origin.</param>
		/// <param name="vector">The vector to display.</param>
		public VisualLogVectorEntry DrawVector(Vector2 screenPos, Vector2 vector)
		{
			return this.DrawVector(screenPos.X, screenPos.Y, vector.X, vector.Y);
		}
		/// <summary>
		/// Draws a vector in world space. A vector can be thought of as an arrow pointing in a certain direction.
		/// It is useful for displaying velocities or directions.
		/// </summary>
		/// <param name="worldX">The vectors world origin.</param>
		/// <param name="worldY">The vectors world origin.</param>
		/// <param name="worldZ">The vectors world origin.</param>
		/// <param name="vectorX">The vector to display.</param>
		/// <param name="vectorY">The vector to display.</param>
		public VisualLogVectorEntry DrawVector(float worldX, float worldY, float worldZ, float vectorX, float vectorY)
		{
			VisualLogVectorEntry entry = new VisualLogVectorEntry();
			entry.Origin = new Vector3(worldX, worldY, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			entry.Vector = new Vector2(vectorX, vectorY);
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a vector in world space. A vector can be thought of as an arrow pointing in a certain direction.
		/// It is useful for displaying velocities or directions.
		/// </summary>
		/// <param name="worldPos">The vectors world origin.</param>
		/// <param name="vector">The vector to display.</param>
		public VisualLogVectorEntry DrawVector(Vector3 worldPos, Vector2 vector)
		{
			return this.DrawVector(worldPos.X, worldPos.Y, worldPos.Z, vector.X, vector.Y);
		}
		/// <summary>
		/// Draws a connection between two screen space points.
		/// </summary>
		/// <param name="screenX1"></param>
		/// <param name="screenY1"></param>
		/// <param name="screenX2"></param>
		/// <param name="screenY2"></param>
		public VisualLogConnectionEntry DrawConnection(float screenX1, float screenY1, float screenX2, float screenY2)
		{
			VisualLogConnectionEntry entry = new VisualLogConnectionEntry();
			entry.PosA = new Vector3(screenX1, screenY1, 0.0f);
			entry.PosB = new Vector3(screenX2, screenY2, 0.0f);
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a connection between two screen space points.
		/// </summary>
		/// <param name="screenPos1"></param>
		/// <param name="screenPos2"></param>
		public VisualLogConnectionEntry DrawConnection(Vector2 screenPos1, Vector2 screenPos2)
		{
			return this.DrawConnection(screenPos1.X, screenPos1.Y, screenPos2.X, screenPos2.Y);
		}
		/// <summary>
		/// Draws a connection between two world space points. Both points need to be on the same Z plane.
		/// </summary>
		/// <param name="worldX1"></param>
		/// <param name="worldY1"></param>
		/// <param name="worldZ"></param>
		/// <param name="worldX2"></param>
		/// <param name="worldY2"></param>
		public VisualLogConnectionEntry DrawConnection(float worldX1, float worldY1, float worldZ, float worldX2, float worldY2)
		{
			VisualLogConnectionEntry entry = new VisualLogConnectionEntry();
			entry.PosA = new Vector3(worldX1, worldY1, worldZ);
			entry.PosB = new Vector3(worldX2, worldY2, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a connection between two world space points. Both points need to be on the same Z plane.
		/// </summary>
		/// <param name="worldPos1"></param>
		/// <param name="worldPos2"></param>
		public VisualLogConnectionEntry DrawConnection(Vector3 worldPos1, Vector2 worldPos2)
		{
			return this.DrawConnection(worldPos1.X, worldPos1.Y, worldPos1.Z, worldPos2.X, worldPos2.Y);
		}
		/// <summary>
		/// Draws a convex polygon in screen space.
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		/// <param name="polygon"></param>
		public VisualLogPolygonEntry DrawPolygon(float screenX, float screenY, Vector2[] polygon)
		{
			VisualLogPolygonEntry entry = new VisualLogPolygonEntry();
			entry.Pos = new Vector3(screenX, screenY, 0.0f);
			entry.Vertices = polygon;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a convex polygon in screen space.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <param name="polygon"></param>
		public VisualLogPolygonEntry DrawPolygon(Vector2 screenPos, Vector2[] polygon)
		{
			return this.DrawPolygon(screenPos.X, screenPos.Y, polygon);
		}
		/// <summary>
		/// Draws a convex polygon in world space.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="worldZ"></param>
		/// <param name="polygon"></param>
		public VisualLogPolygonEntry DrawPolygon(float worldX, float worldY, float worldZ, Vector2[] polygon)
		{
			VisualLogPolygonEntry entry = new VisualLogPolygonEntry();
			entry.Pos = new Vector3(worldX, worldY, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			entry.Vertices = polygon;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a convex polygon in world space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="polygon"></param>
		public VisualLogPolygonEntry DrawPolygon(Vector3 worldPos, Vector2[] polygon)
		{
			return this.DrawPolygon(worldPos.X, worldPos.Y, worldPos.Z, polygon);
		}
		/// <summary>
		/// Draws a text in screen space. Unlike other log entries, texts retain a constant rotation and size.
		/// Only the point of their origin is transformed regularly.
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		/// <param name="text"></param>
		public VisualLogTextEntry DrawText(float screenX, float screenY, string text)
		{
			VisualLogTextEntry entry = new VisualLogTextEntry();
			entry.Pos = new Vector3(screenX, screenY, 0.0f);
			entry.Text = text;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a text in screen space. Unlike other log entries, texts retain a constant rotation and size.
		/// Only the point of their origin is transformed regularly.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <param name="text"></param>
		public VisualLogTextEntry DrawText(Vector2 screenPos, string text)
		{
			return this.DrawText(screenPos.X, screenPos.Y, text);
		}
		/// <summary>
		/// Draws a text in world space. Unlike other log entries, texts retain a constant rotation and size.
		/// Only the point of their origin is transformed regularly.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="worldZ"></param>
		/// <param name="text"></param>
		public VisualLogTextEntry DrawText(float worldX, float worldY, float worldZ, string text)
		{
			VisualLogTextEntry entry = new VisualLogTextEntry();
			entry.Pos = new Vector3(worldX, worldY, worldZ);
			entry.Anchor = VisualLogAnchor.World;
			entry.Text = text;
			this.Draw(entry);
			return entry;
		}
		/// <summary>
		/// Draws a text in world space. Unlike other log entries, texts retain a constant rotation and size.
		/// Only the point of their origin is transformed regularly.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="text"></param>
		public VisualLogTextEntry DrawText(Vector3 worldPos, string text)
		{
			return this.DrawText(worldPos.X, worldPos.Y, worldPos.Z, text);
		}

		/// <summary>
		/// Removes all previously added items from the log. Don't use this
		/// when you're dealing with a global log, the system already calls
		/// this method when appropriate.
		/// </summary>
		public void Clear()
		{
			this.entries.Clear();
		}
		/// <summary>
		/// Updates all currently existing log items. Don't use this
		/// when you're dealing with a global log, the system already calls
		/// this method when appropriate.
		/// </summary>
		public void Update()
		{
			for (int i = entries.Count - 1; i >= 0; i--)
			{
				entries[i].Update();
				if (!entries[i].IsAlive) entries.RemoveAt(i);
			}
		}
	}
}
