using Battleships.Web.DataAccess.Models.Base;

namespace Battleships.Web.DataAccess
{
    public class Grid
    {
        public const int gridSize = 10;
        public char[,] playerGrid { get; set; } = new char[gridSize, gridSize];
        public char[,] hiddenGrid { get; set; } = new char[gridSize, gridSize];
        public List<Ship> Ships { get; set; }

        public Grid()
        {
            Ships = new List<Ship>
            {
                new Ship { name = "Battleship", size = 5, coordinates = new List<(int, int)>(), hits = 0 },
                new Ship { name = "Destroyer1", size = 4, coordinates = new List<(int, int)>(), hits = 0 },
                new Ship { name = "Destroyer2", size = 4, coordinates = new List<(int, int)>(), hits = 0 }
            };

            // Initialize the grid with water
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    playerGrid[i, j] = '~';
                    hiddenGrid[i, j] = '~';
                }
            }
        }
    }
}
