using System.Collections.Generic;
using Game.State.Models;

namespace Game.Controllers
{
    public class LoadGameConfigController
    {
        public LoadGameConfigController(List<ILoadData<GameConfig>> services, GameConfig gameConfig)
        {
            foreach (var service in services) service.LoadFrom(gameConfig);
        }
    }
}