using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Components.Renderers;

namespace BasicMenu
{
	/// <summary>
	/// This Component implements an Event-based MenuController.
	/// Pros: Logic gets called only when an event is fired, allows better timing due to the presence of 
	/// OnInit and OnShutdown.
	/// Cons: Requires to clean up the event listeners once finished.
	/// </summary>
	[RequiredComponent(typeof(Camera))]
	public class EventMenuController : MenuController, ICmpInitializable
	{
		[DontSerialize]
		private EventHandler<Duality.Input.MouseMoveEventArgs> mouseMove;
		[DontSerialize]
		private EventHandler<Duality.Input.MouseButtonEventArgs> buttonDown;
		[DontSerialize]
		private Vector2 mousePosition;
		[DontSerialize]
		private MenuComponent currentComponent;

		public EventMenuController()
		{
			mouseMove = new EventHandler<Duality.Input.MouseMoveEventArgs>(Mouse_Move);
			buttonDown = new EventHandler<Duality.Input.MouseButtonEventArgs>(Button_Down);
		}

		public void OnInit(Component.InitContext context)
		{
			// listening for mouse Move and ButtonDown events
			if (context == InitContext.Activate)
			{
				// since I know I'm being activated, I can switch to the StartingMenu here
				this.SwitchToMenu(this.StartingMenu);

				DualityApp.Mouse.Move += mouseMove;
				DualityApp.Mouse.ButtonDown += buttonDown;
			}
		}

		public void OnShutdown(Component.ShutdownContext context)
		{
			// remember to clean up the events on Deactivate - needs to be more careful
			if (context == ShutdownContext.Deactivate)
			{
				DualityApp.Mouse.Move -= mouseMove;
				DualityApp.Mouse.ButtonDown -= buttonDown;
			}
		}

		void Mouse_Move(object sender, Duality.Input.MouseMoveEventArgs e)
		{
			mousePosition = e.Pos;

			// check all MenuComponents under the mouse and sort them by Z,
			// to find the one nearest to the Camera
			MenuComponent hoveredComponent = this.GameObj.ParentScene.FindComponents<MenuComponent>()
				.Where(mc => mc.GameObj.Active && mc.GetAreaOnScreen().Contains(mousePosition))
				.OrderBy(mc => mc.GameObj.Transform.Pos.Z)
				.FirstOrDefault();

			// I found my hovered menu component.. is it different from the current one?
			if (hoveredComponent != currentComponent)
			{
				// if the old one is not null, leave it.
				if (currentComponent != null)
				{
					currentComponent.MouseLeave();
				}

				// if the new one is not null, enter it.
				if (hoveredComponent != null)
				{
					hoveredComponent.MouseEnter();
				}
			}

			// set the current component to the hovered one.
			currentComponent = hoveredComponent;
		}

		void Button_Down(object sender, Duality.Input.MouseButtonEventArgs e)
		{
			// did I click the left button and am I hovering a component? do something!
			if (e.Button == Duality.Input.MouseButton.Left && currentComponent != null)
			{
				currentComponent.DoAction();
			}
		}
	}
}
