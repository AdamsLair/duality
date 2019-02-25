﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duality;
using Duality.Resources;

using Duality.Editor.Properties;

namespace Duality.Editor
{
	public enum SandboxState
	{
		Inactive,
		Playing,
		Paused
	}

	public static class Sandbox
	{
		private	static bool					stateChange	= false;
		private	static int					singleSteps	= 0;
		private	static int					sceneFreeze	= 0;
		private	static SandboxState			state		= SandboxState.Inactive;
		private static bool					askUnsaved	= true;
		private static ContentRef<Scene>	startScene	= null;

		public	static	event	EventHandler	Entering		= null;
		public	static	event	EventHandler	Leave			= null;
		public	static	event	EventHandler	Paused			= null;
		public	static	event	EventHandler	Unpausing		= null;
		public	static	event	EventHandler	StateChanged	= null;
		
		public static SandboxState State
		{
			get { return state; }
		}
		public static bool IsActive
		{
			get { return state != SandboxState.Inactive; }
		}
		public static bool IsFreezed
		{
			get { return sceneFreeze > 0; }
		}
		public static bool IsChangingState
		{
			get { return stateChange; }
		}
		
		
		internal static void Init()
		{
			Scene.Leaving += Scene_Leaving;
		}
		internal static void Terminate()
		{
			Stop();
			Scene.Leaving -= Scene_Leaving;
		}

		public static bool Play()
		{
			if (state == SandboxState.Playing) return true;

			// If the current Scene is unsaved when entering sandbox mode, ask whether this should be done before
			if (askUnsaved && state == SandboxState.Inactive && Scene.Current.IsRuntimeResource && !Scene.Current.IsEmpty)
			{
				askUnsaved = false;
				DialogResult result = MessageBox.Show(DualityEditorApp.MainForm,
					GeneralRes.Msg_EnterSandboxUnsavedScene_Desc,
					GeneralRes.Msg_EnterSandboxUnsavedScene_Caption,
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);
				if (result == DialogResult.Cancel) return false;
				if (result == DialogResult.Yes) DualityEditorApp.SaveCurrentScene(false);
			}

			stateChange = true;
			if (state == SandboxState.Paused)
			{
				OnUnpausingSandbox();
				state = SandboxState.Playing;
			}
			else
			{
				OnEnteringSandbox();

				// Save the current scene
				DualityEditorApp.SaveCurrentScene();
				ContentRef<Scene> activeScene = Scene.Current;
				startScene = activeScene;

				// Leave the current Scene
				Scene.SwitchTo(null, true);
				
				// Force later Scene reload by disposing it
				if (!activeScene.IsRuntimeResource)
					activeScene.Res.Dispose();

				state = SandboxState.Playing;
				DualityApp.SwitchExecutionContext(DualityApp.ExecutionContext.Game);

				// (Re)Load Scene - now in playing context
				Scene.SwitchTo(activeScene);
			}

			OnSandboxStateChanged();
			stateChange = false;
			return true;
		}
		public static void Pause()
		{
			if (state != SandboxState.Playing) return;
			stateChange = true;

			state = SandboxState.Paused;

			OnPausedSandbox();
			OnSandboxStateChanged();
			stateChange = false;
		}
		public static void Stop()
		{
			if (state == SandboxState.Inactive) return;
			stateChange = true;

			ContentRef<Scene> activeScene = Scene.Current;

			// Leave the current Scene
			Scene.SwitchTo(null, true);

			// Force later Scene reload by disposing it
			if (!activeScene.IsRuntimeResource)
				activeScene.Res.Dispose();

			// Stop all audio that might not have been taken care of manually by the user
			DualityApp.Sound.StopAll();

			Time.TimeScale = 1.0f; // Reset time scale
			Time.Resume(true);     // Reset any previously (user-)generated time freeze events
			state = SandboxState.Inactive;
			DualityApp.SwitchExecutionContext(DualityApp.ExecutionContext.Editor);

			// Check if the Scene we started the sandbox with is still valid, and switch back to it.
			if (startScene.IsAvailable)
			{
				Scene.SwitchTo(startScene);
			}
			// Otherwise, just switch back to the previously active Scene. This can happen if the 
			// start scene was a runtime-only Scene that was active while a plugin reload occurred.
			else if (activeScene.IsAvailable)
			{
				Scene.SwitchTo(activeScene);
			}

			OnLeaveSandbox();
			OnSandboxStateChanged();
			stateChange = false;
		}
		public static void Step()
		{
			singleSteps++;
		}
		public static void Faster()
		{
			Time.TimeScale *= 2.0f;
		}
		public static void Slower()
		{
			Time.TimeScale /= 2.0f;
		}
		public static void Freeze()
		{
			sceneFreeze++;
		}
		public static void UnFreeze()
		{
			sceneFreeze--;
		}

		internal static bool TakeSingleStep()
		{
			if (singleSteps > 0)
			{
				singleSteps--;
				return true;
			}
			return false;
		}

		private static void OnEnteringSandbox()
		{
			if (Entering != null)
				Entering(null, EventArgs.Empty);
		}
		private static void OnLeaveSandbox()
		{
			if (Leave != null)
				Leave(null, EventArgs.Empty);
		}
		private static void OnPausedSandbox()
		{
			if (Paused != null)
				Paused(null, EventArgs.Empty);
		}
		private static void OnUnpausingSandbox()
		{
			if (Unpausing != null)
				Unpausing(null, EventArgs.Empty);
		}
		private static void OnSandboxStateChanged()
		{
			if (StateChanged != null)
				StateChanged(null, EventArgs.Empty);
		}

		private static void Scene_Leaving(object sender, EventArgs e)
		{
			if (stateChange) return;
			if (state == SandboxState.Inactive)
			{
				askUnsaved = true;
				return;
			}

			// Force later Scene reload by disposing it
			string curPath = null;
			if (!string.IsNullOrEmpty(Scene.Current.Path))
			{
				curPath = Scene.CurrentPath;
				Scene.Current.Dispose();
			}
		}
	}
}
