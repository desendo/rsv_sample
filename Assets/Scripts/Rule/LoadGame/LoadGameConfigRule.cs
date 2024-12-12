using System.Collections.Generic;
using Game.State.Models;

namespace Game.Rules
{
    public class LoadGameConfigRule
    {
        public LoadGameConfigRule(List<ILoadData<GameConfig>> services, GameConfig gameConfig)
        {
            foreach (var service in services) service.LoadFrom(gameConfig);
        }
    }
}