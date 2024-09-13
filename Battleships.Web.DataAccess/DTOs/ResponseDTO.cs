namespace Battleships.Web.DataAccess.DTOs
{
    public class ResponseDTO
    {
    }

    public class GameResponse
    {
        public char[,] grid { get; set; }
        public int totalHits { get; set; }
        public int totalMisses { get; set; }
        public int totalShots { get; set; }
        public int battleshipHits { get; set; }
        public int destroyer1Hits { get; set; }
    }
}
