using Microsoft.AspNetCore.Components.Web;

namespace FlappyBird.Components.Components.Input
{
    public interface IControlInputHandler
    {
        void OnKeyDown(KeyboardEventArgs evt);
    }
}