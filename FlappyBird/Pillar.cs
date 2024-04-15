using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Pillar
    {
        private const int OPENING_SIZE = 5;

        private const double MOVEMENT_PER_TICK = 30.0;

        private double _xPos;

        public bool Passed { get; set; }
        private int XPos { get
            {
                return (int)Math.Round(_xPos);
            }
        }

        private int YPos { get; set; }

        public Pillar(Random rng)
        {
            _xPos = Console.WindowWidth - 1;
            this.YPos = rng.Next(Console.WindowHeight - 1);
            this.Passed = false;
        }

        public int Tick(byte[] framebuffer, Bird faby, double deltaTime, int ars)
        {
            int prevXPos = this.XPos < 0 ? 0 : this.XPos ;
            int retVal = 0;

            if (faby.XPos >= this.XPos && ((this.YPos - OPENING_SIZE) < faby.YPos && faby.YPos < (this.YPos + OPENING_SIZE)) && this.Passed == false) {
                retVal = 1;
            } else if (faby.XPos >= this.XPos && this.Passed == false)
            {
                return -2;
            }

            _xPos -= MOVEMENT_PER_TICK * deltaTime;

            if (this.XPos <= 0)
            {
                for (int i = 0; i < Console.WindowHeight - 2; i++)
                {
                    if (prevXPos < (Console.WindowWidth) && !(i < (this.YPos - OPENING_SIZE) && (this.YPos + OPENING_SIZE) < i)) // Itt van valami amire majd ra kene nezni
                    {
                        framebuffer[(i * Console.WindowWidth) + prevXPos] = (byte)' ';
                    }
                }

                return -1;
            }

            for (int i = 0; i < Console.WindowHeight - 2; i++)
            {
                if (!((this.YPos - OPENING_SIZE) < i && i < (this.YPos + OPENING_SIZE)) && prevXPos < (Console.WindowWidth))
                {
                    framebuffer[(i * Console.WindowWidth) + prevXPos] = (byte)' ';
                }

                if (Console.WindowWidth < this.XPos)
                {
                    framebuffer[(i * Console.WindowWidth) + this.XPos] = (byte)'#';
                }

                if ((this.YPos - OPENING_SIZE) < i && i < (this.YPos + OPENING_SIZE))
                {
                    framebuffer[(i * Console.WindowWidth) + this.XPos] = (byte)' ';
                }
                else
                {
                    framebuffer[(i * Console.WindowWidth) + this.XPos] = (byte)'#';
                }
            }

            return retVal;  
        }
    }
}
