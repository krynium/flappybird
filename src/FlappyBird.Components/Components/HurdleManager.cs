using System.Numerics;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FlappyBird.Components.Components
{
    public class HurdleManager
    {
        private readonly GameScene scene;
        private readonly Timer hurdleTimer = new Timer(1330);
        private int hurdleCount = 0;

        public HurdleManager(GameScene scene)
        {
            this.scene = scene;
            hurdleTimer.Elapsed += HurdleTimerTick;
            Start();
        }

        public Vector2 NewHurdleVelocity { get; set; } = new Vector2(-3.40f, 0);

        private void HurdleTimerTick(object sender, ElapsedEventArgs e)
        {
            Interlocked.Increment(ref hurdleCount);
            var hurdle = new Hurdle(scene,hurdleCount);
            hurdle.SetVelocity(NewHurdleVelocity);
            scene.AddChild(hurdle);
        }

        public void Stop()
        {
            if (hurdleTimer.Enabled)
            {
                hurdleTimer.AutoReset = true;
                hurdleTimer.Stop();
            }
        }

        public void Start()
        {
            if (!hurdleTimer.Enabled)
            {
                hurdleTimer.Start();
            }
        }

        public void GenerateNew()
        {
            HurdleTimerTick(null, null);
        }
    }
}