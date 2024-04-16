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

        /// <summary>
        /// Elhagyta-e a madar mar az oszlopot?
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Az oszlop X pozicioja kerekitve, hogy megjelenitheto legyen.
        /// </summary>
        private int XPos { get
            {
                return (int)Math.Round(_xPos);
            }
        }

        /// <summary>
        /// Az oszlop Y pozicioja.
        /// </summary>
        private int YPos { get; set; }

        /// <summary>
        /// Az oszlop konstruktorja, legeneral egy random Y poziciot,
        /// ahol a nyilas el lesz helyezve.
        /// </summary>
        /// <param name="rng">Egy random szam generator, amivel tudunk generalni Y poziciot.</param>
        public Pillar(Random rng)
        {
            _xPos = Console.WindowWidth - 1;
            // Nem akarjuk, hogy a nyilas a kepen kivul keruljon ki
            this.YPos = rng.Next(OPENING_SIZE + 1, Console.WindowHeight - 1);
            this.Passed = false;
        }

        /// <summary>
        /// Egy tick (azaz frame renderelesenek) alkalmaval, arrebb mozditja egy adott szammal 
        /// az oszlopot, majd magvaltoztatja a kiirando kepet (framebuffert), emellett megnezi, hogy
        /// a madar nem-e utkozott bele az oszlopba.
        /// </summary>
        /// <param name="framebuffer">A Framebuffer, amibe be kell rajzolni a kialakult kepet.</param>
        /// <param name="faby">A madar</param>
        /// <param name="deltaTime">A deltaTime (elozo frame ota eltelt ido) szorzo.</param>
        /// <returns>
        /// -  1-et ha a madar sikeresen atjutott
        /// -  0-at ha csak arrebb mozdult,
        /// - -1-et ha az oszlop kiert a lathato teruletrol
        /// - -2-ot ha a madar beleutkozott az oszlopba
        /// </returns>
        public int Tick(byte[] framebuffer, Bird faby, double deltaTime)
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
                if (prevXPos < Console.WindowWidth) // A biztonsag kedveert
                {
                    for (int i = 0; i < Console.WindowHeight - 2; i++)
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
