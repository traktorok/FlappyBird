using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Bird
    {
        private const int JUMP_SIZE = 3;
        private const double FALL_PER_TICK = 0.4;
        private const double JUMP_PER_TICK = 1.0;

        // Az X es Y poziciok invertalva vannak, mert a Console origoja az a bal felso sarok.
        private int _xPos;
        private double _yPos;
        private int _jumpsLeft;

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

        public Bird(int startXPos, double startYPos)
        {
            this._xPos = startXPos;
            this._yPos = startYPos;
            this._jumpsLeft = 0;
        }

        public void Jump()
        {
            this._jumpsLeft += JUMP_SIZE;
        }

        public int Tick(byte[] framebuffer)
        {
            if (0 < this.YPos)
                framebuffer[(this.YPos * Console.WindowWidth) + this.XPos] = (byte)' ';

            if (0 < _jumpsLeft)
            {
                this._yPos -= JUMP_PER_TICK;
                this._jumpsLeft--;
            } else
            {
                this._yPos += FALL_PER_TICK;
            }

            if (0 < this.YPos)
                framebuffer[(this.YPos * Console.WindowWidth) + this.XPos] = (byte)'@';

            if ((Console.WindowHeight - 1) < YPos)
            {
                return -1;
            }

            return 0;
        }
    }
}
