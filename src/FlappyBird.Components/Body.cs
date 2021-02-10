using System;
using System.Drawing;
using System.Numerics;

namespace FlappyBird.Components
{
    public class Body
    {
        public ICollider Collider { get; set; }
        public static float DefaultBounceFactor = 0.75f;
        public float BounceFactor { get; set; } = DefaultBounceFactor;

        private Vector2 gravity = Vector2.Zero;
        public float Mass { get; set; } = 1;

        private Vector2 acceleration = Vector2.Zero;

        public Vector2 Position { get; set; }
        public SizeF Size { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; } = new Vector2(float.MaxValue, float.MaxValue);
        public RectangleF Bounds => new RectangleF(Position.X, Position.Y, Size.Width, Size.Height);

        public Body()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
        }

        public void ApplyPhysics(float delta)
        {
            Velocity += ComputeNetForce();
            Velocity = Vector2.Clamp(Velocity, Velocity, MaxVelocity);

            Position += Velocity;
            Bounce();
            acceleration = Vector2.Zero;

            Vector2 ComputeNetForce()
            {
                
                const float friction = 1.0f;
                var total = (gravity + acceleration) * friction;

                return total;
            }
        }

        private void Bounce()
        {
            if (BounceFactor == 0)
            {
                return;
            }

            if (Position.X + Size.Width > World.Width)
            {
                Position = new Vector2(World.Width - Size.Width, Position.Y);
                //Reverse velocity on x axis
                Velocity = new Vector2(-Velocity.X * BounceFactor, Velocity.Y);
            }
            else if (Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
                //Reverse velocity on x axis
                Velocity = new Vector2(-Velocity.X * BounceFactor, Velocity.Y);
            }

            if (Position.Y + Size.Height > World.Height)
            {
                Position = new Vector2(Position.X, World.Height - Size.Height);
                //Reverse velocity on y axis
                Velocity = new Vector2(Velocity.X, -Velocity.Y * BounceFactor);
            }
            else if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, 0);
                //Reverse velocity on y axis
                Velocity = new Vector2(Velocity.X, -Velocity.Y * BounceFactor);
            }
        }

        public void AddForce(Vector2 force)
        {
            //F = ma
            var newacc = acceleration + force / Mass;
            Console.WriteLine($"Accellaration: {newacc}");
            //TODO CLAPM Max Accellaration
            acceleration = newacc;
        }

        public void SetGravity(float val)
        {
            gravity = new Vector2(0, val);
        }
    }

    public class RectangleBody : Body
    {
    }
}