using Battleships.Web.DataAccess;
using Battleships.Web.DataAccess.DTOs;

namespace Battleships.Web.Service.GameService
{
    public interface IGameServiceRepository
    {
        GameResponse GetPlayerGrid();
        Shot PostFireShot(string coordinate);
        GameResponse RestartGame();
    }
}