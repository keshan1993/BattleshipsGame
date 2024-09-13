namespace Battleships.Web.DataAccess
{
    public class Ship
    {
        public string? name { get; set; }
        public int size { get; set; }
        public List<(int x, int y)>? coordinates { get; set; }
        public int? hits { get; set; }

        public bool isSunk => hits >= size;
    }
}
