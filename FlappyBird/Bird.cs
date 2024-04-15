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
