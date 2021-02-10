using System;
using System.Drawing;
using System.Numerics;

namespace FlappyBird.Components.Components
{
    [ComponentType(typeof(HurdleComponent))]
    public class Hurdle : GameObject
    {
        public int Id { get; }
        public Pipe TopPipe { get; }
        public Pipe BottomPipe { get; }
        private const int PipeGap = 135;

        public Hurdle(GameScene scene,int id) : base(scene)
        {
            this.Id = id;
            SetSize(new SizeF(52, World.Height));
            SetPosition(new Vector2(scene.Size.Width + 125, 0));
            TopPipe = (Pipe) AddChild(CreateTopPipe());
            BottomPipe = (Pipe) AddChild(CreateBottomPipe(TopPipe));
            Collider = null;
            //Collider = new HurdleCollider(TopPipe,BottomPipe);
            SetBounceFactor(0);
        }

        private Pipe CreateTopPipe()
        {
            var pipe = new Pipe(this);
            var topHeight = GetRandomHeight();
            pipe.SetPosition(new Vector2(Position.X, Position.Y));
            pipe.SetSize(new SizeF(Size.Width, topHeight));
            pipe.Tag = "Top Pipe";

            return pipe;
        }

        private static readonly Random rand = new Random();

        private int GetRandomHeight()
        {
            //105-180
            var low = World.Height * 0.2; // 20 % of total height
            var high = World.Height * 0.4;
            return rand.Next((int) low, (int) high);
        }

        private Pipe CreateBottomPipe(Pipe topPipe)
        {
            var bottomPipeHeight = Size.Height - PipeGap - topPipe.Size.Height;
            var pipe = new Pipe(this);
            pipe.SetPosition(new Vector2(Position.X, Size.Height - bottomPipeHeight));
            pipe.SetSize(new SizeF(Size.Width, Position.Y + bottomPipeHeight));
            pipe.Tag = "Bottom Pipe";
            return pipe;
        }

        public sealed override void SetVelocity(Vector2 velocity)
        {
            base.SetVelocity(velocity);
            TopPipe.SetVelocity(velocity);
            BottomPipe.SetVelocity(velocity);
        }

        public sealed override void SetBounceFactor(float val)
        {
            base.SetBounceFactor(val);
            TopPipe.SetBounceFactor(val);
            BottomPipe.SetBounceFactor(val);
        }
    }
}