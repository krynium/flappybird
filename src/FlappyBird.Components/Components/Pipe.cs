using System.Diagnostics;
using FlappyBird.Components.Colliders;

namespace FlappyBird.Components.Components
{
    //does not have component because this is managed by hurdle
    [DebuggerDisplay("{Tag}")]
    public class Pipe : GameObject
    {
        public string Tag { get; set; }

        public Pipe(Hurdle hurdle) : base(hurdle?.Scene)
        {
            Collider = new DefaultCollider(this);
        }
    }
}