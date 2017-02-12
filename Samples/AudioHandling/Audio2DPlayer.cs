using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Audio;
using Duality.Resources;

namespace AudioHandling
{
	public class Audio2DPlayer : Component, ICmpUpdatable
	{
		private ContentRef<Sound> sound = null;
		private float volume = 1.0f;
		private float pitch = 1.0f;
		private float lowpass = 1.0f;
		private float panning = 0.0f;

		[DontSerialize] private SoundInstance instance = null;


		public ContentRef<Sound> Sound
		{
			get { return this.sound; }
			set { this.sound = value; }
		}
		[EditorHintRange(0.0f, 2.0f)]
		public float Volume
		{
			get { return this.volume; }
			set { this.volume = value; }
		}
		[EditorHintRange(0.5f, 2.0f)]
		public float Pitch
		{
			get { return this.pitch; }
			set { this.pitch = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float Lowpass
		{
			get { return this.lowpass; }
			set { this.lowpass = value; }
		}
		[EditorHintRange(-1.0f, 1.0f)]
		public float Panning
		{
			get { return this.panning; }
			set { this.panning = value; }
		}


		void ICmpUpdatable.OnUpdate()
		{
			if (this.instance == null || this.instance.Disposed)
			{
				this.instance = DualityApp.Sound.PlaySound(this.sound);
				this.instance.Looped = true;
			}
			this.instance.Volume = this.volume;
			this.instance.Lowpass = this.lowpass;
			this.instance.Pitch = this.pitch;
			this.instance.Panning = this.panning;
		}
	}
}
