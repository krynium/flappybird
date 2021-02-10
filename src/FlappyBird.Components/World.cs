using System.Drawing;

namespace FlappyBird.Components
{
    public class World
    {
        public static int Width = 450;
        public static int Height = 380;
        public static SizeF Size => new SizeF(Width, Height);
    }
}