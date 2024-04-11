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

        private double _xPos;
        
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
        }

        public int Tick(byte[] framebuffer, Bird faby, int deltaTime)
        {
            int prevXPos = this.XPos;
            int retVal = 0;

            if (faby.XPos == this.XPos && ((this.YPos - OPENING_SIZE) < faby.YPos && faby.YPos < (this.YPos + OPENING_SIZE))) {
                retVal = 1;
            } else if (faby.XPos == this.XPos)
            {
                return -2;
            }

            _xPos -= Math.Pow(1.0, deltaTime * 2);

            if (this.XPos < 0)
            {
                for (int i = 0; i < Console.WindowHeight - 1; i++)
                {
                    if (prevXPos < (Console.WindowWidth - 1) && !((this.YPos - OPENING_SIZE) < i && i < (this.YPos + OPENING_SIZE)))
                    {
                        framebuffer[(i * Console.WindowWidth) + prevXPos] = (byte)' ';
                    }
                }

                return -1;
            }

            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                if (!((this.YPos - OPENING_SIZE) < i && i < (this.YPos + OPENING_SIZE)) && prevXPos < (Console.WindowWidth - 1))
                {
                    framebuffer[(i * Console.WindowWidth) + prevXPos] = (byte)' ';
                }

                if ((Console.WindowWidth - 1) < this.XPos)
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
