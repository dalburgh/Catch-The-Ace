var app = new CatchTheAceSimulator.App();

// Application entry point
// Usage: .\bin\Debug\net6.0\CatchTheAce [true]
if (args.Length > 0 && args[0].ToLower() == "true")
{
    app.Run(true);
}
else
{
    app.Run(false);
}

namespace CatchTheAceSimulator
{
    class App
    {
        // Application runtime
        public void Run(bool argPassed)
        {
            Console.WriteLine("\nWelcome to the Catch the Ace Simulation!\n");

            Console.WriteLine("This program will simulate the Catch the Ace game for a given number of years");
            Console.WriteLine("to determine the probability of the Ace being caught on the last week of the year.\n");

            // Create an unshuffled deck of cards
            int[] numberedDeck = CreateUnshuffledDeck();

            int yearsToSimulate = 0;
            // Get the number of years to simulate from the user. If the user enters a number less than 1, the user will be reprompted.
            while (yearsToSimulate == 0)
            {
                Console.Write("How many years would you like to run the simulation? : ");
                yearsToSimulate = getYears();
            };

            // Start the diagnostic stopwatch
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Run the simulation
            int wins = Simulate(numberedDeck, yearsToSimulate);

            // Stop the stopwatch
            watch.Stop();

            // If the user passed in the argument "true", print simulation diagnostics
            if (argPassed)
            {
                float elapsedMs = watch.ElapsedMilliseconds;
                float winPercentage = (float)wins / yearsToSimulate * 100;

                Console.WriteLine($"Program Execution Time:    {elapsedMs} ms");
                Console.WriteLine($"Number of Years Simulated: {yearsToSimulate}");
                Console.WriteLine($"Number of Wins:            {wins}");
                Console.WriteLine($"Win Percentage:            {winPercentage}%");
            }

            Console.Write($"Press any key to exit.");
            Console.ReadKey();
            System.Environment.Exit(0);
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
            catch (FormatException)
            {
                PrintColoredLine("Please enter a valid number.", ConsoleColor.Red);
                return 0;
            }
            catch (Exception e)
            {
                PrintColoredLine("An unknown error has occured, please try running the program again.", ConsoleColor.Red);
                PrintColoredLine(e.Message, ConsoleColor.Red);
                System.Environment.Exit(1);
                return 0;
            }
        }

        // Simulates the Catch the Ace game for a given number of years
        //
        // Returns
        //  int: the number of times the Ace was caught on the last week of the year
        // 
        // Parameters:
        //  int[] deck: an array of integers representing a deck of cards
        //  int yearsToSimulate: the number of years to simulate
        int Simulate(int[] deck, int yearsToSimulate)
        {
            int wins = 0;
            for (int year = 1; year <= yearsToSimulate; year++)
            {
                deck = ShuffleDeck(deck);
                for (int week = 1; week <= 52; week++)
                {
                    if (deck[week - 1] == 52)
                    {
                        if (week == 52)
                        {
                            PrintColoredLine($"Year {year}: the Ace was caught on week {week}, the last week of the year.", ConsoleColor.Green);
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

        int[] CreateUnshuffledDeck()
        {
            int[] deck = new int[52];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = i + 1;
            }
            return deck;
        }

        int[] ShuffleDeck(int[] deck)
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

        void PrintColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, Console.ForegroundColor);
            Console.ResetColor();
        }
    }
}
