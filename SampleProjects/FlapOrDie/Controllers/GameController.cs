using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components.Physics;
using Duality.Components;
using Duality.Resources;
using Duality.Components.Renderers;
using FlapOrDie.Components;

namespace FlapOrDie.Controllers
{
    [RequiredComponent(typeof(Camera))]
    public class GameController : Component, ICmpUpdatable
    {
        [DontSerialize]
        private Vector2 deltaPos;

        private PlayerController player;

        public PlayerController Player
        {
            get { return this.player; }
            set { this.player = value; }
        }

        private float baseSpeed;
        private float pointsMultiplier;
        private float pointsGapVariance;
        private ushort lastFramePoints;

        private ContentRef<Prefab> obstaclePrefab;
        private GameObject gameOverOverlay;
		private BackgroundScroller bgScroller;

        public float BaseSpeed
        {
            get { return this.baseSpeed; }
            set { this.baseSpeed = value; }
        }

        public float PointsMultiplier
        {
            get { return this.pointsMultiplier; }
            set { this.pointsMultiplier = value; }
        }

        public float PointsGapVariance
        {
            get { return this.pointsGapVariance; }
            set { this.pointsGapVariance = value; }
        }

        public ContentRef<Prefab> ObstaclePrefab
        {
            get { return this.obstaclePrefab; }
            set { this.obstaclePrefab = value; }
        }

        public GameObject GameOverOverlay
        {
            get { return this.gameOverOverlay; }
            set { this.gameOverOverlay = value; }
        }

		public BackgroundScroller BackgroundScroller
        {
            get { return this.bgScroller; }
			set { this.bgScroller = value; }
        }

		private TextRenderer scoreText;

		public TextRenderer ScoreTextRenderer
		{
			get { return this.scoreText; }
			set { this.scoreText = value; }
		}

        public void Reset()
        {
            // reset all the game data
            GameOverOverlay.Active = false;

            this.lastFramePoints = 0;
            this.player.Reset();

            foreach (GameObject obstacle in this.GameObj.ParentScene.FindGameObjects<Tags.Obstacle>())
            {
                obstacle.DisposeLater();
            }
        }

        void ICmpUpdatable.OnUpdate()
        {
            this.scoreText.Text.SourceText = String.Format("Score: {0}", player.Points);

            deltaPos.X = this.baseSpeed + (player.Points * this.pointsMultiplier);
            deltaPos.X *= Time.MsPFMult * Time.TimeMult / 1000;

			this.bgScroller.Update(deltaPos.X);

            IEnumerable<GameObject> obstacles = this.GameObj.ParentScene.FindGameObjects<Tags.Obstacle>();
            foreach(GameObject obstacle in obstacles)
            {
                obstacle.Transform.MoveBy(-this.deltaPos);
                if (obstacle.Transform.Pos.X < -FlapOrDieCorePlugin.HalfWidth - 50)
                {
                    obstacle.DisposeLater();
                }
            }
            if (!player.IsDead)
            {
                if (obstacles.Count() == 0)
                {
                    AddObstacle(0);
                }
                if (player.Points > this.lastFramePoints)
                {
                    AddObstacle(player.Points * this.pointsGapVariance);
                }
            }
            else
            {
                GameOverOverlay.Active = true;
            }

            this.lastFramePoints = player.Points;
        }


        private void AddObstacle(float variance)
        {
            variance = Math.Min(variance, 200);

            Vector3 startPosition = new Vector3(FlapOrDieCorePlugin.HalfWidth + 50, MathF.Rnd.NextFloat(-variance, variance), 0);
            GameObject newObstacle = this.obstaclePrefab.Res.Instantiate();
            newObstacle.Transform.Pos = startPosition;
			foreach(AnimSpriteRenderer asr in newObstacle.GetComponentsInChildren<AnimSpriteRenderer>())
			{
				asr.AnimFirstFrame = MathF.Rnd.Next(asr.SharedMaterial.Res.MainTexture.Res.BasePixmap.Res.AnimFrames) + 1;
			}

            this.GameObj.ParentScene.AddObject(newObstacle);
        }
    }
}
