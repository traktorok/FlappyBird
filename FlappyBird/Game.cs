using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FlappyBird
{
    class Game
    {
        private const int FRAME_RATE = 10;

        public byte[] Framebuffer { get; set; }
        
        private List<Pillar> Pillars { get; set; }

        private Bird Faby { get; set; } // A Google szerint a madár neve Faby.

        private Stream _stdOut;

        public Game()
        {
            Console.CursorVisible = false;

            int width = Console.WindowWidth;
            int height = Console.WindowHeight - 1;

            this.Framebuffer = new byte[width * height];

            _stdOut = Console.OpenStandardOutput(Console.WindowWidth * (Console.WindowHeight - 1));

            int birdStartPosX = width / 2;
            int birdStartPosY = height / 2;

            this.Faby = new Bird(birdStartPosX, birdStartPosY);
            this.Framebuffer[CalculatePosition(birdStartPosX, birdStartPosY)] = (byte)'@';

            this.Render();
        }

        public void Run()
        {
            Console.ReadKey();

            bool running = true;
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while (running)
            {
                Thread.Sleep((1000 / FRAME_RATE) - (int)((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - now));
                this.Render();
                now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (Console.KeyAvailable)
                {
                    var consoleKey = Console.ReadKey();

                    if (consoleKey.Key == ConsoleKey.Escape)
                    {
                        running = false;
                        break;
                    }
                    else if (consoleKey.Key == ConsoleKey.Spacebar)
                    {
                        Faby.Jump();
                    }
                }

                Faby.Tick(this.Framebuffer);

                foreach (var pillar in Pillars)
                {
                    pillar.Tick(this.Framebuffer);
                }
                
                Console.SetCursorPosition(0, 0);
            }
        }

        private int CalculatePosition(int x, int y)
        {
            return (y * Console.WindowWidth) + x;
        }


        private void Render()
        {
            _stdOut.Write(this.Framebuffer, 0, this.Framebuffer.Length);
        }

    }
}
