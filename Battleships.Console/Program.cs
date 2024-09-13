class Program
{
    static readonly int gridSize = 10;
    static readonly char[,] grid = new char[gridSize, gridSize]; // Player's view
    static readonly char[,] ships = new char[gridSize, gridSize]; // Actual ship locations
    static int battleshipHits = 0, destroyer1Hits = 0, destroyer2Hits = 0;
    static int totalHits = 0;
    static int totalMisses = 0;
    static int totalShots = 0;

    static void Main()
    {
        Console.WriteLine("Welcome to Battleships!");

        InitializeGrid();
        PlaceShips();

        while (battleshipHits < 5 || destroyer1Hits < 4 || destroyer2Hits < 4)
        {
            DisplayGrid();
            Console.Write("\nEnter your target: ");
            string input = Console.ReadLine().ToUpper();
            ProcessShot(input);
        }

        Console.WriteLine("Congratulations! You've sunk all the ships 💥");
        Console.WriteLine($"\nGame Summary:");
        Console.WriteLine($"Total Shots Fired: {totalShots}");
        Console.WriteLine($"Total Hits: {totalHits}");
        Console.WriteLine($"Total Misses: {totalMisses}");
    }

    static void InitializeGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = '~'; // Water
                ships[i, j] = '~'; // Hidden ship locations
            }
        }
    }

    static void PlaceShips()
    {
        Random rand = new Random();

        // Place 1 Battleship (5 squares)
        PlaceShip(rand, 5, 'B');

        // Place 2 Destroyers (4 squares)
        PlaceShip(rand, 4, 'D');
        PlaceShip(rand, 4, 'D');
    }

    static void PlaceShip(Random rand, int size, char shipSymbol)
    {
        bool placed = false;
        while (!placed)
        {
            int x = rand.Next(gridSize);
            int y = rand.Next(gridSize);
            bool horizontal = rand.Next(2) == 0; // Randomly choose orientation

            if (CanPlaceShip(x, y, size, horizontal))
            {
                for (int i = 0; i < size; i++)
                {
                    if (horizontal)
                        ships[x, y + i] = shipSymbol;
                    else
                        ships[x + i, y] = shipSymbol;
                }
                placed = true;
            }
        }
    }

    static bool CanPlaceShip(int x, int y, int size, bool horizontal)
    {
        if (horizontal)
        {
            if (y + size > gridSize) return false;
            for (int i = 0; i < size; i++)
                if (ships[x, y + i] != '~') return false;
        }
        else
        {
            if (x + size > gridSize) return false;
            for (int i = 0; i < size; i++)
                if (ships[x + i, y] != '~') return false;
        }
        return true;
    }

    static void DisplayGrid()
    {
        Console.WriteLine("\n   A B C D E F G H I J");
        for (int i = 0; i < gridSize; i++)
        {
            Console.Write(i + 1);
            if (i + 1 < 10) Console.Write(" ");
            for (int j = 0; j < gridSize; j++)
            {
                Console.Write(" " + grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    static void ProcessShot(string input)
    {
        if (input.Length < 2 || input.Length > 3)
        {
            Console.WriteLine("Invalid input. Please enter a valid coordinate (e.g: A5).");
            return;
        }

        // Check if the first character is a valid letter (A-J)
        if (input[0] < 'A' || input[0] > 'J')
        {
            Console.WriteLine("Invalid input. The column must be a letter from A to J.");
            return;
        }

        // Check if the rest of the input is a valid number
        if (!int.TryParse(input.Substring(1), out int row) || row < 1 || row > gridSize)
        {
            Console.WriteLine("Invalid input. The row must be a number between 1 and 10.");
            return;
        }

        int x = int.Parse(input.Substring(1)) - 1; // Row
        int y = input[0] - 'A'; // Column

        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
        {
            Console.WriteLine("Invalid target. Please try again.");
            return;
        }

        if (grid[x, y] == 'H' || grid[x, y] == 'M')
        {
            Console.WriteLine("You already fired at this location. Try another target.");
            return;
        }

        totalShots++;

        if (ships[x, y] == 'B')
        {
            Console.WriteLine("Hit! You hit the battleship.");
            grid[x, y] = 'H';
            battleshipHits++;
            totalHits++;
            if (battleshipHits == 5) Console.WriteLine("You've sunk the battleship!");
        }
        else if (ships[x, y] == 'D')
        {
            Console.WriteLine("Hit! You hit a destroyer.");
            grid[x, y] = 'H';
            totalHits++;
            if (destroyer1Hits < 4) destroyer1Hits++;
            else destroyer2Hits++;
            if (destroyer1Hits == 4) Console.WriteLine("You've sunk the first destroyer!");
            if (destroyer2Hits == 4) Console.WriteLine("You've sunk the second destroyer!");
        }
        else
        {
            Console.WriteLine("Miss.");
            grid[x, y] = 'M';
            totalMisses++;
        }
    }
}
