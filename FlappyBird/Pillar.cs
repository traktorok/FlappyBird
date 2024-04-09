using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Pillar
    {
        private int XPos { get; set; }

        private int YPos { get; set; }

        public Pillar(Random rng)
        {
            this.XPos = Console.BufferWidth - 1;
            this.YPos = rng.Next(Console.BufferHeight - 1); 
        }

        public void Tick(byte[] framebuffer)
        {

        }
    }
}
