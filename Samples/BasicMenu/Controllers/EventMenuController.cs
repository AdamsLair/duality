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
			this.mouseMove = new EventHandler<Duality.Input.MouseMoveEventArgs>(this.Mouse_Move);
			this.buttonDown = new EventHandler<Duality.Input.MouseButtonEventArgs>(this.Button_Down);
		}

		public void OnActivate()
		{
			// since I know I'm being activated, I can switch to the StartingMenu here
			this.SwitchToMenu(this.StartingMenu);

			DualityApp.Mouse.Move += this.mouseMove;
			DualityApp.Mouse.ButtonDown += this.buttonDown;
		}

		public void OnDeactivate()
		{
			// remember to clean up the events on Deactivate - needs to be more careful
			DualityApp.Mouse.Move -= this.mouseMove;
			DualityApp.Mouse.ButtonDown -= this.buttonDown;
		}

		void Mouse_Move(object sender, Duality.Input.MouseMoveEventArgs e)
		{
			this.mousePosition = e.Pos;

			// check all MenuComponents under the mouse and sort them by Z,
			// to find the one nearest to the Camera
			MenuComponent hoveredComponent = this.Scene.FindComponents<MenuComponent>()
				.Where(mc => mc.GameObj.Active && mc.GetAreaOnScreen().Contains(this.mousePosition))
				.OrderBy(mc => mc.GameObj.Transform.Pos.Z)
				.FirstOrDefault();

			// I found my hovered menu component.. is it different from the current one?
			if (hoveredComponent != this.currentComponent)
			{
				// if the old one is not null, leave it.
				if (this.currentComponent != null)
				{
					this.currentComponent.MouseLeave();
				}

				// if the new one is not null, enter it.
				if (hoveredComponent != null)
				{
					hoveredComponent.MouseEnter();
				}
			}

			// set the current component to the hovered one.
			this.currentComponent = hoveredComponent;
		}

		void Button_Down(object sender, Duality.Input.MouseButtonEventArgs e)
		{
			// did I click the left button and am I hovering a component? do something!
			if (e.Button == Duality.Input.MouseButton.Left && this.currentComponent != null)
			{
				this.currentComponent.DoAction();
			}
		}
	}
}
