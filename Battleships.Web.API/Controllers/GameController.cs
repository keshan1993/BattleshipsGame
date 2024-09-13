using Battleships.Web.DataAccess.DTOs;
using Battleships.Web.Service.GameService;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private IGameServiceRepository _gameService;

        public GameController(IGameServiceRepository gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("load-grid")]
        public ActionResult GetGrid()
        {
            return Ok(_gameService.GetPlayerGrid());
        }

        [HttpPost("fire-shot")]
        public ActionResult FireShot([FromBody] CoordinateRequest request)
        {
            var result = _gameService.PostFireShot(request.Coordinate);
            return Ok(result);
        }

        [HttpPost("restart")]
        public IActionResult RestartGame()
        {
            return Ok(_gameService.RestartGame());
        }
    }
}
