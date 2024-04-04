using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.CodeDom;

namespace FlappyBird
{
    class Program
    {
        private const int FRAME_RATE = 30;

        static void Main(string[] args)
        {
            byte[] frameBuf = new byte[Console.WindowWidth * (Console.WindowHeight - 1)];

            Console.CursorVisible = false;
            var stdOut = Console.OpenStandardOutput(Console.WindowWidth * (Console.WindowHeight - 1));
            
            bool running = true;

            Bird bird = new Bird(Console.WindowWidth / 2, Console.WindowHeight / 2);
            frameBuf[(bird.YPos * Console.WindowWidth) + bird.XPos] = bird.BirdGFX;
            stdOut.Write(frameBuf, 0, Console.WindowWidth * (Console.WindowHeight - 1));

            Console.ReadKey();

            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while (running)
            {
                Thread.Sleep((1000/FRAME_RATE) - (int)((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - now));
                stdOut.Write(frameBuf, 0, Console.WindowWidth * (Console.WindowHeight - 1));
                now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (Console.KeyAvailable)
                {
                    var consoleKey = Console.ReadKey();

                    if (consoleKey.Key == ConsoleKey.Escape)
                    {
                        running = false;
                        break;
                    } else if (consoleKey.Key == ConsoleKey.Spacebar)
                    {
                        bird.Jump();
                    }
                }

                if (0 < bird.YPos)
                    frameBuf[(bird.YPos * Console.WindowWidth) + bird.XPos] = (byte)' ';
                
                bird.Tick();

                if (0 < bird.YPos)
                    frameBuf[(bird.YPos * Console.WindowWidth) + bird.XPos] = bird.BirdGFX;

                Console.SetCursorPosition(0, 0);
            }

            Console.Clear();
            Console.WriteLine("GameOver");

            Console.ReadKey();
        }
    }
}
