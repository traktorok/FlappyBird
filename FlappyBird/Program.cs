﻿using System;
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
            string jatekTitle = "Flappy Bird";
            string jatekDescription = "Nyomd meg (egyesével) a Space-t hogy ugorj, az Esc-el ki tudsz lépni.";
            string kezdesDescription = "Ugorj a kezdéshez!";
            string gameOverText = "Game Over!";
            string kilepesText = "Nyomd meg az R-t az újrainditáshoz, vagy bármi mást a kilépeshez.";

            try
            {
                while (true)
                {
                    Game birdGame = new Game();

                    try
                    {
                        Console.SetCursorPosition((Console.WindowWidth - jatekTitle.Length) / 2, Console.WindowHeight / 4);
                        Console.WriteLine(jatekTitle);

                        Console.SetCursorPosition((Console.WindowWidth - jatekDescription.Length) / 2, (Console.WindowHeight / 4) * 3);
                        Console.WriteLine(jatekDescription);

                        Console.SetCursorPosition((Console.WindowWidth - kezdesDescription.Length) / 2, (Console.WindowHeight / 4) * 3 + 1);
                        Console.WriteLine(kezdesDescription);
                    } catch (Exception)
                    {
                        // Nem irjuk ki, ha nem eleg szeles a konzol
                    }

                    int score = birdGame.Run();

                    if (score == -1)
                        return;

                    string pontokText = $"Pontok: {score}";

                    Console.Clear();

                    try
                    {
                        Console.SetCursorPosition((Console.WindowWidth - gameOverText.Length) / 2, Console.WindowHeight / 2 - 1);
                        Console.WriteLine(gameOverText);

                        Console.SetCursorPosition((Console.WindowWidth - pontokText.Length) / 2, Console.WindowHeight / 2);
                        Console.WriteLine(pontokText);

                        Console.SetCursorPosition((Console.WindowWidth - kilepesText.Length) / 2, Console.WindowHeight / 2 + 1);
                        Console.WriteLine(kilepesText);
                    } catch (Exception)
                    {
                        // Nem irjuk ki, ha nem eleg szeles a konzol
                    }

                    Thread.Sleep(1000);

                    var key = Console.ReadKey();
                    if (key.Key != ConsoleKey.R)
                    {
                        break;
                    }
                }
            } catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
