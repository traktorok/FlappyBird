using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Bird
    {
        private const int JUMP_SIZE = 4;
        private const double FALL_PER_TICK = 15.0;
        private const double JUMP_PER_TICK = 15.0;
        private const double JUMP_REDUCE_PER_TICK = 25.0;

        // Az X es Y poziciok invertalva vannak, mert a Console origoja az a bal felso sarok.
        private int _xPos;
        private double _yPos;
        private double _jumpsLeft;

        /// <summary>
        /// Az Y pozicio alapbol double, azaz tort szam, de
        /// ahhoz, hogy kirajzolhassuk fel kell kerekitenunk
        /// egy egesz szamra, az YPos property ezt teszi az _yPos
        /// privat valtozoval.
        /// </summary>
        public int YPos
        {
            get
            {
                return (int)(Math.Round(_yPos));
            }
        }


        public int XPos
        {
            get { return _xPos; }
        }

        /// <summary>
        /// Beallitja a madar alappoziciojat, es alap allapotat
        /// </summary>
        /// <param name="startXPos">X alappozicio</param>
        /// <param name="startYPos">Y alappozicio</param>
        public Bird(int startXPos, double startYPos)
        {
            this._xPos = startXPos;
            this._yPos = startYPos;
            this._jumpsLeft = 0;
        }

        /// <summary>
        /// Felugrassza a madarat, mivel a felugras tobb
        /// frame-en keresztul tortenik, igy csak beallitja,
        /// hogy mennyi ideig kell ugrani.
        /// </summary>
        public void Jump()
        {
            this._jumpsLeft += JUMP_SIZE;
        }

        /// <summary>
        /// Egy tick (azaz frame renderelesenek) alkalmaval, feljebb vagy lejjebb viszi a madar poziciojat,
        /// attol fuggoen, hogy van-e folyamatban ugras, vagy nincs, majd a kirajzolando
        /// kepet a madarrol elhelyezi a framebufferben, az elozo pozicio rajzat pedig
        /// letorli. Emellett a mozgasoknal figyelembe veszi a deltaTimeot, igy meg akkor
        /// is ha lassabb gepen futtatjuk, ugyanannyit mozdul el.
        /// </summary>
        /// <param name="framebuffer">A Framebuffer, amibe be kell rajzolni a kialakult kepet.</param>
        /// <param name="deltaTime">A deltaTime (elozo frame ota eltelt ido) szorzo.</param>
        /// <returns>-1-et ad vissza, ha a madar utkozott a talajjal, 0-at, ha nem.</returns>
        public int Tick(byte[] framebuffer, double deltaTime)
        {
            int prevYPos = this.YPos;

            if (0 < _jumpsLeft)
            {
                this._yPos -= JUMP_PER_TICK * deltaTime;
                this._jumpsLeft -= JUMP_REDUCE_PER_TICK * deltaTime;
            } else
            {
                this._yPos += FALL_PER_TICK * deltaTime;
            }

            if (this.YPos == prevYPos)
            {
                return 0;
            }

            if (0 < prevYPos)
                framebuffer[(prevYPos * Console.WindowWidth) + this.XPos] = (byte)' ';

            if ((Console.WindowHeight - 3) < YPos)
                return -1;

            if (0 < this.YPos)
                framebuffer[(this.YPos * Console.WindowWidth) + this.XPos] = (byte)'@';

            return 0;
        }
    }
}
