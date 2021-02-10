using System.Collections.Generic;
using System.Linq;

namespace FlappyBird.Components
{
    public class CollisionDetail
    {
        private readonly List<GameObject> collisions = new List<GameObject>();
        public GameObject Source { get; set; }

        public IEnumerable<GameObject> GetCollisions()
        {
            return collisions;
        }

        public CollisionDetail(GameObject source)
        {
            Source = source;
        }

        public bool HasCollision()
        {
            return collisions.Any();
        }

        public void AddCollision(GameObject obj)
        {
            collisions.Add(obj);
        }
    }
}