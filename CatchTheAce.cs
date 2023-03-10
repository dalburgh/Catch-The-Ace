var app = new CatchTheAceSimulator.App();

// Application entry point
// Usage: .\bin\Debug\net6.0\CatchTheAce [true] [full]
if (args.Length > 0)
{
    bool diagBool = Array.Exists(args, element => element.ToLower() == "true");
    bool logsBool = Array.Exists(args, element => element.ToLower() == "full");
    app.Run(diagBool, logsBool);
}
else
{
    app.Run();
}

namespace CatchTheAceSimulator
{
    class App
    {
        // Application runtime
        public void Run(bool diagnosticsBool = false, bool logsBool = false)
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
                yearsToSimulate = getYears();
            };

            // Start the diagnostic stopwatch
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Run the simulation, print logs if the user passed in the argument "full"
            int wins = logsBool ? Simulate(numberedDeck, yearsToSimulate, true) : Simulate(numberedDeck, yearsToSimulate);

            // Stop the stopwatch
            watch.Stop();

            // Calculate the win percentage
            float winPercentage = (float)wins / yearsToSimulate * 100;
            Console.WriteLine($"Win Percentage:            {winPercentage}%");
            
            // If the user passed in the argument "true", print simulation diagnostics
            if (diagnosticsBool)
            {
                float elapsedMs = watch.ElapsedMilliseconds;

                Console.WriteLine($"Number of Years Simulated: {yearsToSimulate}");
                Console.WriteLine($"Number of Aces in week 52: {wins}");
                Console.WriteLine($"Program Execution Time:    {elapsedMs} ms");
            }

            Console.Write($"Press any key to exit.");
            Console.ReadKey();
            System.Environment.Exit(0);
        }


        // Gets the number of years to simulate from the user
        //
        // Returns
        //  int: the number of times the Ace was caught on the last week of the year
        // 
        // Parameters:
        //  int[] deck: an array of integers representing a deck of cards
        //  int yearsToSimulate: the number of years to simulate
        int getYears()
        {
            try
            {
                Console.Write("How many years would you like to run the simulation? : ");
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
        //  bool logs: whether or not to print logs to the console
        int Simulate(int[] deck, int yearsToSimulate, bool logs = false)
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
                            if (logs)
                            {
                                PrintColoredLine($"Year {year}: the Ace was caught on week {week}, the last week of the year.", ConsoleColor.Green);
                            }
                            wins++;
                        }
                        else
                        {
                            if (logs)
                            {
                                PrintColoredLine($"Year {year}: the Ace was caught on week {week}.", ConsoleColor.Red);
                            }
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
