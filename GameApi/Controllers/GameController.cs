using GameApi.Database;
using GameLib;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GameApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameData _gameDB = new GameData();
        private List<GameResults> _gameList = new List<GameResults>();

        public GameController()
        {
            _gameList = _gameDB.GetAllGamesFromDB();
        }

    }
}
