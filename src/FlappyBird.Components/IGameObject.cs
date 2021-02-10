namespace FlappyBird.Components
{
    public interface IGameObject
    {
        void RenderFrame(float delta);
        void UpdateGame(float delta);
    }
}