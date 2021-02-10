using Microsoft.AspNetCore.Components;

namespace FlappyBird.Components.Components
{
    public abstract class GameComponent : ComponentBase
    {
        public GameObject GameObject { get; set; }

        public virtual void Refresh()
        {
            InvokeAsync(StateHasChanged);
        }
    }
}