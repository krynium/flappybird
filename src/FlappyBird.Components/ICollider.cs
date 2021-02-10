namespace FlappyBird.Components
{
    public interface ICollider
    {
        CollisionDetail DetectCollision(GameObject target);
    }
}