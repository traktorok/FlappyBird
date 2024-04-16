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
        private const int FRAME_RATE_CAP = 500;

        public byte[] Framebuffer { get; set; }
        
        private List<Pillar> Pillars { get; set; }

        private Bird Faby { get; set; } // A Google szerint a madár neve Faby.

        private Stream _stdOut;

        private Random _rng;
        public int Counter { set; get; }

        /// <summary>
        /// A Game konstruktor, elokesziti a konzolt, beallitja a jatek kezdeti
        /// allapotat.
        /// </summary>
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

            for (int i = 0; i < Console.WindowWidth; i++) {
                this.Framebuffer[CalculatePosition(i, Console.WindowHeight - 2)] = (byte)'#';
            }

            this.Pillars = new List<Pillar>();

            this._rng = new Random(); 

            this.Pillars.Add(new Pillar(this._rng));

            this.Counter = 0;

            this.Render();
        }

        /// <summary>
        /// A jatek fo ciklusa, amely figyeli a lenyomott billentyuket, es kirajzoltatja a kepet.
        /// </summary>
        /// <returns>A kikerult oszlopok szama.</returns>
        public int Run()
        {
            Console.ReadKey();

            bool running = true;

            // Ezzel merem az idot a frame-ek kozott, mert a DateTime valamiert nem
            // mukodott.
            Stopwatch measure = new Stopwatch();
            measure.Start();

            while (running)
            {
                // A legutobbi frame renderelesetol eltelt ido
                double deltaTime = (measure.ElapsedMilliseconds / 1000.0);
                deltaTime = deltaTime == 0 ? 0.001 : deltaTime; // Erdekes deltaTime megoldas
                measure = new Stopwatch();
                measure.Start();

                // Kirajzoljuk a framebuffert
                this.Render();

                // Van-e lenyomva billenttyu, ha igen, akkor,
                // ha Esc lepjen ki, ha space ugrassa a madarat
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

                Thread.Sleep(1); // Ha a deltaTime 0 lenne (gyakoribb, mind hittem), akkor azert,
                                 // hogy a key inputot ne ugorjuk at ez kell

                // Vegigmegyunk az oszlopokon es mindegyiknek meghivjuk a Tick metodusat, hogy
                // igy arrebb menjenek
                for (int i = 0; i < this.Pillars.Count; i++)
                {
                    switch (this.Pillars[i].Tick(this.Framebuffer, this.Faby, deltaTime, this.Pillars.Count))
                    {
                        case -1:
                            Pillars.RemoveAt(i);
                            break;
                        case -2:
                            running = false;
                            break;
                        case 1:
                            if (!this.Pillars[i].Passed)
                            {
                                Pillars.Add(new Pillar(this._rng));
                                this.Counter++;
                                this.Pillars[i].Passed = true;
                            }
                            break;
                    }

                }

                // A madaron is meghivjuk a Tick fuggvenyt, ha -1-et ad vissza
                // akkor beleutkozott valamibe, es igy vege a jateknak
                if (Faby.Tick(this.Framebuffer, deltaTime) == -1)
                    return this.Counter;

                Console.SetCursorPosition(0, 0);
            }

            return this.Counter;
        }

        /// <summary>
        /// Atkonvertalja a 2D array poziciot, 1 dimenzios
        /// array poziciova
        /// </summary>
        /// <param name="x">2D pozicio X</param>
        /// <param name="y">2D pozicio Y</param>
        /// <returns>1D pozicio</returns>
        private int CalculatePosition(int x, int y)
        {
            return (y * Console.WindowWidth) + x;
        }

        /// <summary>
        /// Kirajzolja a Framebuffer array tartalmat.
        /// </summary>
        private void Render()
        {
            _stdOut.Write(this.Framebuffer, 0, this.Framebuffer.Length);
        }

    }
}
