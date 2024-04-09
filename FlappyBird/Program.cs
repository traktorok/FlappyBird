using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlappyBird
{
    class Program
    {
        private const int FRAME_RATE = 30;

        static void Main(string[] args)
        {
            Game birdGame = new Game();
            birdGame.Run();
        }
    }
}
