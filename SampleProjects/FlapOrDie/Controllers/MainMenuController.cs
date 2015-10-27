using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components.Physics;
using Duality.Components;
using Duality.Resources;

namespace FlapOrDie.Controllers
{
    [RequiredComponent(typeof(Camera))]
    public class MainMenuController : Component, ICmpUpdatable
    {
        void ICmpUpdatable.OnUpdate()
        {
            if(DualityApp.Keyboard.KeyHit(Key.Escape))
            {
                DualityApp.Terminate();
            }
            if(DualityApp.Keyboard.KeyHit(Key.Space))
            {
                ContentRef<Scene> gameScene = ContentProvider.RequestContent<Scene>(@"Data\GameScene.Scene.res");
                gameScene.Res.FindComponent<GameController>().Reset();

                Scene.SwitchTo(gameScene);
            }
        }
    }
}
