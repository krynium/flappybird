using System.Numerics;
using Microsoft.AspNetCore.Components.Web;

namespace FlappyBird.Components.Components.Input
{
    public class BirdControlInputHandler : IControlInputHandler
    {
        private readonly Bird bird;
        
        public BirdControlInputHandler(Bird bird)
        {
            this.bird = bird;
        }

        public void OnKeyDown(KeyboardEventArgs evt)
        {
            if (evt.Code == "Space" || evt.Key == "ArrowUp")
            {
                bird.ApplyForce(new Vector2(0f, -45));
            }

            if (evt.Code == "ArrowDown")
            {
                bird.ApplyForce(new Vector2(0f, 5f));
            }
        }
        }
}