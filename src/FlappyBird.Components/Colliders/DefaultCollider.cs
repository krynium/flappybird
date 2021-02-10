using FlappyBird.Components.Components;

namespace FlappyBird.Components.Colliders
{
    public class DefaultCollider : ICollider
    {
        protected readonly GameObject SourceObject;

        public DefaultCollider(GameObject sourceObject)
        {
            SourceObject = sourceObject;
        }

        private CollisionDetail Check(GameObject source, GameObject target, CollisionDetail detail = null)
        {
            //TODO: rewrite this logic
            detail ??= new CollisionDetail(source);

            if (source == target)
            {
                return detail;
            }

            if (source.Collider == null)
            {
                return detail;
            }

            if (target.Collider == null)
            {
                LoopChildren();
            }
            else
            {
                var src = source.Bounds;
                var trg = target.Bounds;

                if (src.X < trg.X + trg.Width &&
                    src.X + src.Width > trg.X &&
                    src.Y < trg.Y + trg.Height &&
                    src.Y + src.Height > trg.Y)
                {
                    detail.AddCollision(target);
                    LoopChildren();
                }
            }

            return detail;

            void LoopChildren()
            {
                foreach (var child in target.GetChildrens())
                {
                    Check(source, child, detail);
                }
            }
        }

        public virtual CollisionDetail DetectCollision(GameObject target)
        {
            var detail = new CollisionDetail(SourceObject);
            detail = Check(SourceObject, target, detail);
            return detail;
        }
    }

    public class HurdleCollider : ICollider
    {
        private readonly Pipe top;
        private readonly Pipe bottom;

        public HurdleCollider(Pipe top,Pipe bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }
        public CollisionDetail DetectCollision(GameObject target)
        {
            if (top.Collider != null)
            {
                var result = top.Collider.DetectCollision(target);
                if (result.HasCollision())
                {
                    return result;
                }
            }

            if (bottom.Collider != null)
            {
                var result = bottom.Collider.DetectCollision(target);
                if (result.HasCollision())
                {
                    return result;
                }
            }
            return new CollisionDetail(target);
        }
    }
}