using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace FlappyBird.Components.Components
{
    public class GameEngine
    {
        private readonly GameEngineComponent component;
        public GameScene Scene { get; private set; }
        private const int FPS = 60;
        private const int TimerIntervalMilli = 1000 / FPS;
        public bool IsGameOver { get; private set; }
        private Queue<KeyboardEventArgs> keyboardevents = new Queue<KeyboardEventArgs>();

        private CancellationTokenSource stopToken = new CancellationTokenSource();

        public GameEngine(GameEngineComponent component)
        {
            this.component = component;
            Scene = new GameScene(this);
        }

        public bool IsRunning { get; private set; }
        public bool Debug { get; set; }

        private long lastRender;

        public async void Start()
        {
            this.IsGameOver = false;
            var timer = new Stopwatch();
            timer.Start();
            while (!stopToken.IsCancellationRequested)
            {
                IsRunning = true;
                var now = timer.ElapsedMilliseconds;
                var elapsed = now - lastRender;
                var delta = elapsed / 10000f;
                ProcessKeyboardEvents(delta);
                Scene.UpdateGame(delta);
                Scene.RenderFrame(delta);
                component?.Refresh();
                lastRender = now;
                await Task.Delay(10);
            }

            lastRender = 0;
            IsRunning = false;
            timer.Stop();
            stopToken = new CancellationTokenSource();
        }

        private void ProcessKeyboardEvents(float delta)
        {
            while (keyboardevents.Count > 0)
            {
                var evt = keyboardevents.Dequeue();
                Scene.KeyDown(evt);
            }
        }

        public void PushKeyboardEvents(KeyboardEventArgs evt)
        {
            keyboardevents.Enqueue(evt);
            if (evt.Key == "p")
            {
                if (IsRunning)
                {
                    Stop();
                }
                else
                {
                    Start();
                }
            }

            if (evt.Key == "d")
            {
                Debug = !Debug;
            }

            if (evt.Key == "r")
            {
                Restart();
            }
        }

        public void Stop()
        {
            stopToken.Cancel();
            keyboardevents = new Queue<KeyboardEventArgs>();
        }

        public void GameOver()
        {
            Scene.StopCreatingHurdles();
            //Drop bird a bit faster
            //Scene.GetGameObjects<Bird>().First().ApplyForce(new Vector2(0, 2f));
            this.IsGameOver = true;
        }

        public void Restart()
        {
            Stop();
            Scene = new GameScene(this);
            Start();
        }
    }
}