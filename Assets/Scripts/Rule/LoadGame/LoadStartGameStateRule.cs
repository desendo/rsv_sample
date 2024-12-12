using System.Collections.Generic;
using Game.State.Config;
using Game.State.Data;
using Game.State.Models;

namespace Game.Rules
{
    public class LoadStartGameStateRule
    {
        public LoadStartGameStateRule(List<ILoadData<StateData>> services,
            StateDataContainer defaultStateContainer)
        {
            foreach (var service in services) service.LoadFrom(defaultStateContainer.Data);
        }
    }
}