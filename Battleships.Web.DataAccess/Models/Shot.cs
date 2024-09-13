using Battleships.Web.DataAccess.Models.Base;

namespace Battleships.Web.DataAccess
{
    public class Shot
    {
        public string? coordinate { get; set; }
        public bool? hit { get; set; }
        public string? resultMessage { get; set; }
        public bool? IsSunk { get; set; }

    }
}
