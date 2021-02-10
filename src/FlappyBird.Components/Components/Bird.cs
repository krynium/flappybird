using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using FlappyBird.Components.Colliders;
using FlappyBird.Components.Components.Input;


namespace FlappyBird.Components.Components
{
    [ComponentType(typeof(BirdComponent))]
    public class Bird : GameObject
    {
        public string FlapPosition { get; set; } = "mid-flap";
        private DateTime lastRefreshed;
        public BirdDirection XDirection { get; private set; }
        public BirdDirection YDirection { get; private set; }

        public Bird(GameScene scene, float mass = 3.5f) : base(scene)
        {
            SetSize(new SizeF(34, 24));
            Body.Mass = mass;
            Collider = new DefaultCollider(this);
            Body.SetGravity(0.5f);
            Body.MaxVelocity = new Vector2(0, 17f);
            InputHandler = new BirdControlInputHandler(this);
        }

        public int Collissions { get; private set; }
        public int Score { get; private set; }

        public override void RenderFrame(float delta)
        {
            DrawFlaps();
            base.RenderFrame(delta);
        }

        private ScoreCalculator calculator;
        protected override void OnGameUpdate(float delta)
        {
            calculator ??= new ScoreCalculator(this.Scene);

            if (calculator.HurdleCrossed())
            {
                if (!calculator.HadCollisions)
                {
                    Score++;
                }
                else
                {
                    Collissions++;
                }
                
                //get calculator for next hurdle
                calculator=new ScoreCalculator(this.Scene);
            }
            else
            {
                calculator.DetectCollision();
                if (calculator.HadCollisions)
                {
                    this.Scene.Engine.GameOver();
                }
            }
        }
        

       


        

        private void DrawFlaps()
        {
            if (!isFlying)
            {
                FlapPosition = "mid-flap";
                return;
            }

            var now = DateTime.Now;
            var diff = now - lastRefreshed;
            if (diff > TimeSpan.FromMilliseconds(180))
            {
                lastRefreshed = now;
                if (FlapPosition == "mid-flap")
                {
                    FlapPosition = "down-flap";
                }
                else if (FlapPosition == "down-flap")
                {
                    FlapPosition = "up-flap";
                }
                else if (FlapPosition == "up-flap")
                {
                    FlapPosition = "mid-flap";
                }
            }
        }

        private bool isFlying = true;

        protected override void OnMove(Vector2 oldPosition, Vector2 newPosition)
        {
            isFlying = oldPosition != newPosition;
            base.OnMove(oldPosition, newPosition);
            XDirection = newPosition.X < oldPosition.X
                ? BirdDirection.Backward
                : BirdDirection.Forward;

            YDirection = newPosition.Y < oldPosition.Y
                ? BirdDirection.Backward
                : BirdDirection.Forward;
        }

       


        private class ScoreCalculator
        {
            private readonly GameScene scene;
            readonly List<CollisionDetail> collisions=new List<CollisionDetail>();

            private readonly Hurdle processingHurdle;
            private Bird Bird { get; }
            public ScoreCalculator(GameScene scene)
            {
                this.scene = scene;
                Bird = scene.GetGameObjects<Bird>().First();
                processingHurdle = GetClosestHurdle();
                DetectCollision();
                
                
            }

            
            public void DetectCollision()
            {
                var detectCollision = Bird.Body.Collider.DetectCollision(processingHurdle);
                collisions.Add(detectCollision);
            }

            public bool HadCollisions => collisions.Any(x => x.HasCollision());

            private Hurdle GetClosestHurdle()
            {
                var hurdles = scene.GetGameObjects<Hurdle>();
                var first = hurdles.FirstOrDefault(x => x.Bounds.X > this.Bird.Bounds.X);
                return first;
            }

            public bool HurdleCrossed()
            {
                return processingHurdle.Position.X + processingHurdle.Size.Width < Bird.Position.X;

            }
        }
    }

}