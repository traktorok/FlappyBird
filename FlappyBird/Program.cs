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
        static void Main(string[] args)
        {
            while (true)
            {
                Game birdGame = new Game();
                int score = birdGame.Run();

                Console.Clear();
                Console.WriteLine("GameOver");
                Console.WriteLine($"Pontok: {score}");

                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.R) {
                    break;
                }
            }
        }
    }
}
