var app = new CatchTheAceSimulator.App();

// Application entry point
// Usage: .\bin\Debug\net6.0\CatchTheAce [true] [full]
if (args.Length > 0)
{
    var diagBool = Array.Exists(args, element => element.ToLower() == "true");
    var logsBool = Array.Exists(args, element => element.ToLower() == "full");
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
            // Introduction
            Console.WriteLine("\nWelcome to the Catch the Ace Simulation!\n");

            Console.WriteLine("This program will simulate the Catch the Ace game for a given number of years");
            Console.WriteLine("to determine the probability of the Ace being caught on the last week of the year.\n");

            Deck unshuffledDeck = new Deck();

            var yearsToSimulate = UI.GetYears();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Run the simulation, print logs if the user passed in the argument "full"
            var (wins, data) = logsBool ? Simulate(unshuffledDeck, yearsToSimulate, true) : Simulate(unshuffledDeck, yearsToSimulate);

            watch.Stop();

            // Print yearly logs if the user passed in the argument "full"
            if (logsBool)
            {
                data.ForEach((result) =>
                {
                    var (isWin, message) = ((bool, string))result;

                    if (isWin)
                    {
                        UI.PrintColoredLine(message, ConsoleColor.Green);
                    }
                    else
                    {
                        UI.PrintColoredLine(message, ConsoleColor.Red);
                    }
                });
            }

            // Calculate the win percentage
            decimal winPercentage = Decimal.Round(((decimal)wins / yearsToSimulate) * 100, 5);
            Console.WriteLine($"Win Percentage:            {(winPercentage)}%");

            // If the user passed in the argument "true", print simulation diagnostics
            if (diagnosticsBool)
            {
                var elapsedMs = watch.ElapsedMilliseconds;

                Console.WriteLine($"Number of Years Simulated: {yearsToSimulate}");
                Console.WriteLine($"Number of Aces in week 52: {wins}");
                Console.WriteLine($"Program Execution Time:    {elapsedMs} ms");
            }

            Console.Write($"Press any key to exit.");
            Console.ReadKey();
            System.Environment.Exit(0);
        }

        // Simulates the Catch the Ace game for a given number of years
        //
        // Returns
        //  int: the number of times the Ace was caught on the last week of the year
        // 
        // Parameters:
        //  int[] deck: an array of integers representing a deck of cards
        //  int yearsToSimulate: the number of years to simulate
        //  bool logs: whether or not to add logs to the results list
        (int wins, List<(bool, string)> results) Simulate(Deck deck, int yearsToSimulate, bool logs = false)
        {
            var wins = 0;
            List<(bool, string)> results = new List<(bool, string)>();
            for (var year = 1; year <= yearsToSimulate; year++)
            {
                deck.cards = deck.Shuffle();
                for (var week = 1; week <= Deck.NumberOfCardsInDeck; week++)
                {
                    if (deck.cards[week - 1] == Deck.NumberOfCardsInDeck)
                    {
                        if (week == Deck.NumberOfCardsInDeck)
                        {
                            if (logs)
                            {
                                results.Add((true, $"Year {year}: the Ace was caught on week {week}, the last week of the year."));
                            }
                            wins++;
                        }
                        else
                        {
                            if (logs)
                            {
                                results.Add((false, $"Year {year}: the Ace was caught on week {week}."));
                            }
                        }
                        break;
                    }
                }
            }
            return (wins, results);
        }
    }

    class Deck
    {
        public const int NumberOfCardsInDeck = 52;
        public int[] cards { get; set; }
        public Deck()
        {
            cards = new int[52];
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i] = i + 1;
            }
        }
        public int[] Shuffle()
        {
            Random random = new Random();
            for (var i = 0; i < cards.Length; i++)
            {
                var randomIndex = random.Next(0, this.cards.Length);
                var temp = this.cards[i];
                this.cards[i] = this.cards[randomIndex];
                this.cards[randomIndex] = temp;
            }
            return cards;
        }
    }

    class UI
    {
        public static void PrintColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, Console.ForegroundColor);
            Console.ResetColor();
        }


        // Gets the number of years to simulate from the user
        //
        // Returns
        //  int: the number of years to simulate
        public static int GetYears()
        {
            var answer = 0;
            while (answer <= 0)
            {
                try
                {
                    Console.Write("How many years would you like to run the simulation? : ");
                    answer = int.Parse(Console.ReadLine());
                    if (answer <= 0)
                    {
                        UI.PrintColoredLine("Please enter a number greater than 0.", ConsoleColor.Red);
                    }
                }
                catch (FormatException)
                {
                    UI.PrintColoredLine("Please enter a valid number.", ConsoleColor.Red);
                }
                catch (Exception e)
                {
                    UI.PrintColoredLine("An unknown error has occured, please try running the program again.", ConsoleColor.Red);
                    UI.PrintColoredLine(e.Message, ConsoleColor.Red);
                    System.Environment.Exit(1);
                }
            }
            return answer;
        }
    }
}
