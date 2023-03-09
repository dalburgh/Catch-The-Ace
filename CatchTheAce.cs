using System;

var app = new CatchTheAceSimulator.App();
if (args.Length > 0)
{
    if (args[0] == "true")
    {
        app.Run(true);
    }
    else
    {
        app.Run(false);
    }
}
else
{
    app.Run(false);
}

namespace CatchTheAceSimulator
{
    class App
    {
        public void Run(bool arg)
        {
            Console.WriteLine("\nWelcome to the Catch the Ace Simulation!\n");

            Console.WriteLine("This program will simulate the Catch the Ace game for a given number of years");
            Console.WriteLine("to determine the probability of the Ace being caught on the last week of the year.\n");

            Console.Write("How many times would you like to run the simulation? : ");

            int[] numberedDeck = CreateDeck();
            int yearsToSimulate = 0;

            while (yearsToSimulate == 0)
            {
                yearsToSimulate = getYears();
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();

            int wins = Simulate(numberedDeck, yearsToSimulate);

            watch.Stop();

            if (arg)
            {
                float elapsedMs = watch.ElapsedMilliseconds;
                float winPercentage = (float)wins / yearsToSimulate * 100;

                Console.WriteLine($"Simulation Execution Time: {elapsedMs} ms");
                Console.WriteLine($"Number of Years Simulated: {yearsToSimulate}");
                Console.WriteLine($"Number of Wins:            {wins}");
                Console.WriteLine($"Win Percentage:            {winPercentage}%");
            }

                Console.Write($"Press any key to exit.");
                Console.ReadKey();
        }

        int[] CreateDeck()
        {
            int[] deck = new int[52];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = i + 1;
            }
            return deck;
        }

        int getYears()
        {
            try
            {
                int answer = int.Parse(Console.ReadLine());
                if (answer > 0)
                {
                    return answer;
                }
                else
                {
                    PrintColoredLine("Please enter a number greater than 0.", ConsoleColor.Red);
                    return 0;
                }
            }
            catch (Exception)
            {
                PrintColoredLine("Please enter a valid number.", ConsoleColor.Red);
                return 0;
            }
        }

        int[] Shuffle(int[] deck)
        {
            Random random = new Random();
            for (int i = 0; i < deck.Length; i++)
            {
                int randomIndex = random.Next(0, deck.Length);
                int temp = deck[i];
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
            return deck;
        }

        int Simulate(int[] deck, int yearsToSimulate)
        {
            int wins = 0;
            for (int year = 1; year <= yearsToSimulate; year++)
            {
                deck = Shuffle(deck);
                for (int week = 1; week <= 52; week++)
                {
                    if (deck[week - 1] == 52)
                    {
                        if (week == 52)
                        {
                            PrintColoredLine($"Year {year}: the Ace was caught on week {week}, the last week of the year. Congrats!", ConsoleColor.Green);
                            wins++;
                        }
                        else
                        {
                            PrintColoredLine($"Year {year}: the Ace was caught on week {week}.", ConsoleColor.Red);
                        }
                        break;
                    }
                }
            }
            return wins;
        }

        void PrintColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, Console.ForegroundColor);
            Console.ResetColor();
        }
    }

}
