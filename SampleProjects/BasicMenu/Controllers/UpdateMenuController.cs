using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Components.Renderers;

namespace BasicMenu
{
	[RequiredComponent(typeof(Camera))]
	public class UpdateMenuController : MenuController, ICmpUpdatable
	{
        [DontSerialize]
        private Vector2 mousePosition;
        [DontSerialize]
        private MenuComponent currentComponent;

        void ICmpUpdatable.OnUpdate()
        {
            mousePosition.X = DualityApp.Mouse.X;
            mousePosition.Y = DualityApp.Mouse.Y;

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

            // did I click the left button and am I hovering a component? do something!
            if (DualityApp.Mouse.ButtonHit(Duality.Input.MouseButton.Left) && currentComponent != null)
            {
                currentComponent.DoAction();
            }
        }
    }
}
