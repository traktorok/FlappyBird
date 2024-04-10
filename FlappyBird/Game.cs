using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace FlappyBird
{
    class Game
    {
        private const int FRAME_RATE = 30;

        public byte[] Framebuffer { get; set; }
        
        private List<Pillar> Pillars { get; set; }

        private Bird Faby { get; set; } // A Google szerint a madár neve Faby.

        private Stream _stdOut;

        private Random _rng;

        public int Counter { set; get; }

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

            this.Pillars = new List<Pillar>();

            this._rng = new Random();

            this.Pillars.Add(new Pillar(this._rng));

            this.Counter = 0;

            this.Render();
        }

        public void Run()
        {
            Console.ReadKey();

            bool running = true;

            Stopwatch measure = new Stopwatch();
            measure.Start();
            long dur = measure.ElapsedMilliseconds;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (running)
            {
                // System.Diagnostics.Stopwatch
                
                Thread.Sleep((1000/FRAME_RATE) - (int)(dur - measure.ElapsedMilliseconds));
                measure = new Stopwatch();
                measure.Start();
                this.Render();

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

                //Thread.Sleep(50);

                for (int i = 0; i < this.Pillars.Count; i++)
                {
                    switch (this.Pillars[i].Tick(this.Framebuffer, this.Faby))
                    {
                        case -1:
                            Pillars.RemoveAt(i);
                            break;
                        case -2:
                            running = false;
                            break;
                        case 1:
                            Pillars.Add(new Pillar(this._rng));
                            this.Counter++;
                            break;
                    }

                }
                
                if (Faby.Tick(this.Framebuffer) == -1)
                    return; 
                  
                /*
                a++;
                File.AppendAllText("log.txt", $"{a}. {duration + (1000/FRAME_RATE)} {dur} \n");
                if (1000 < stopwatch.ElapsedMilliseconds)
                {
                    return;
                }
                */

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
