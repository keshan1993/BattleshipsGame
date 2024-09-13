using Battleships.Web.DataAccess;
using Battleships.Web.DataAccess.DTOs;

namespace Battleships.Web.Service.GameService
{
    public class GameService : IGameServiceRepository
    {
        private readonly Random _rand = new Random();
        private static Grid _grid;
        private static int _totalHits = 0;
        private static int _totalMisses = 0;
        private static int _totalShots = 0;
        private static int _battleshipHits = 0;
        private static int _destroyer1Hits = 0;

        public GameService()
        {
            InitializeGame();
        }

        /// <summary>
        /// Restart Game
        /// </summary>
        /// <returns></returns>
        public GameResponse RestartGame()
        {
            InitializeGame();
            return new GameResponse
            {
                grid = _grid.playerGrid,
                totalHits = _totalHits,
                totalMisses = _totalMisses,
                totalShots = _totalShots,
                battleshipHits = _battleshipHits,
                destroyer1Hits = _destroyer1Hits
            };
        }

        /// <summary>
        /// Get player grid ready
        /// </summary>
        /// <returns></returns>
        public GameResponse GetPlayerGrid()
        {
            return new GameResponse
            {
                grid = _grid.playerGrid,
                totalHits = _totalHits,
                totalMisses = _totalMisses,
                totalShots = _totalShots,
                battleshipHits = _battleshipHits,
                destroyer1Hits = _destroyer1Hits
            };
        }

        /// <summary>
        /// firing shot for given cordinate
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Shot PostFireShot(string coordinate)
        {
            // Extract the column and row from the coordinate
            char columnChar = coordinate[0];
            string rowString = coordinate.Substring(1);

            // Validate column
            if (columnChar < 'A' || columnChar > 'J')
            {
                return new Shot { coordinate = coordinate, resultMessage = "Invalid column. Must be between A and J." };
            }

            // Validate and parse row
            if (!int.TryParse(rowString, out int row) || row < 1 || row > 10)
            {
                return new Shot { coordinate = coordinate, resultMessage = "Invalid row. Must be between 1 and 10." };
            }

            int x = row - 1; // Convert row to zero-based index
            int y = columnChar - 'A'; // Convert column to zero-based index

            // Log the calculated indices
            Console.WriteLine($"Firing at: Coordinate: {coordinate}, X: {x}, Y: {y}");

            if (_grid.playerGrid[x, y] != '~')
                return new Shot { coordinate = coordinate, resultMessage = "Already Fired Here!" };

            _totalShots++;

            if (_grid.hiddenGrid[x, y] == '~')
            {
                _grid.playerGrid[x, y] = 'M'; // Miss
                _totalMisses++;

                return new Shot { coordinate = coordinate, hit = false, resultMessage = "Miss!", IsSunk = false };
            }
            else
            {
                _grid.playerGrid[x, y] = 'H'; // Hit
                var shipHit = _grid.Ships.First(s => s.coordinates.Contains((x, y)));

                shipHit.hits++;
                _totalHits++;

                bool IsSunk = false;
                string message = $"Hit! {shipHit.name}";
                if (shipHit.isSunk)
                {
                    message += " has been sunk!";
                    if (shipHit.name == "Battleship")
                    {
                        _battleshipHits++;
                    }
                    else
                    {
                        _destroyer1Hits++;
                    }
                    IsSunk = true;
                }

                return new Shot { coordinate = coordinate, hit = true, resultMessage = message, IsSunk = IsSunk };
            }
        }

        private void InitializeGame()
        {
            _grid = new Grid();
            _totalHits = 0;
            _totalMisses = 0;
            _totalShots = 0;
            _battleshipHits = 0;
            _destroyer1Hits = 0;
            PlaceShips();
        }

        /// <summary>
        /// Ships placement
        /// </summary>
        /// <returns></returns>
        private void PlaceShips()
        {
            foreach (var ship in _grid.Ships)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = _rand.Next(Grid.gridSize);
                    int y = _rand.Next(Grid.gridSize);
                    bool horizontal = _rand.Next(2) == 0;

                    if (CanPlaceShip(x, y, ship.size, horizontal))
                    {
                        for (int i = 0; i < ship.size; i++)
                        {
                            if (horizontal)
                            {
                                _grid.hiddenGrid[x, y + i] = ship.name[0];
                                ship.coordinates.Add((x, y + i));
                            }
                            else
                            {
                                _grid.hiddenGrid[x + i, y] = ship.name[0];
                                ship.coordinates.Add((x + i, y));
                            }
                        }

                        // Log ship placement
                        Console.WriteLine($"Placed {ship.name} at starting coordinates: ({x},{y}), Horizontal: {horizontal}");
                        placed = true;
                    }
                }
            }
            PrintGrid(_grid.hiddenGrid);
        }

        /// <summary>
        /// Print out the hidden grid for debugging
        /// </summary>
        /// <returns></returns>
        private void PrintGrid(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// checking the available slots and placing ships
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        private bool CanPlaceShip(int x, int y, int size, bool horizontal)
        {
            if (horizontal)
            {
                if (y + size > Grid.gridSize) return false;
                for (int i = 0; i < size; i++)
                    if (_grid.hiddenGrid[x, y + i] != '~') return false;
            }
            else
            {
                if (x + size > Grid.gridSize) return false;
                for (int i = 0; i < size; i++)
                    if (_grid.hiddenGrid[x + i, y] != '~') return false;
            }
            return true;
        }
    }
}
