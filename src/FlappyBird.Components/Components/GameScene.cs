using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FlappyBird.Components.Components
{
    [ComponentType(typeof(GameSceneComponent))]
    public class GameScene : GameObject
    {
        public GameEngine Engine { get; }
        public Bird Bird => GetGameObjects<Bird>().FirstOrDefault();
        public HurdleManager HurdleGenerator { get; }

        public GameScene(GameEngine engine) : base(null)
        {
            Engine = engine;
            Body.Size = World.Size;
            HurdleGenerator = new HurdleManager(this);
            HurdleGenerator.GenerateNew();
            IsStatic = true;
            AddBirds();
        }

        private void AddBirds()
        {
            var bird1 = new Bird(this);
            bird1.SetPosition(new Vector2(World.Width*0.2f, 0));

            AddChild(bird1);
            //AddChild(bird2);
        }

        protected override void OnGameUpdate(float delta)
        {
            //Remove expiring hurdles
            foreach (var hurdle in GetOffScreenHurdles())
            {
                RemoveChild(hurdle);
            }
        }

        public IEnumerable<Hurdle> GetOffScreenHurdles()
        {
            return GetGameObjects<Hurdle>()
                .Where(x => x.Position.X < -x.Size.Width).ToList();
        }

        public void StopCreatingHurdles()
        {
            //Freeze hurdles
            foreach (var obj in GetGameObjects<Hurdle>())
            {
                obj.SetVelocity(Vector2.Zero);
            }

            HurdleGenerator.Stop();
        }
    }
}