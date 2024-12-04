using System.Collections.Generic;
using Game.Configuration;
using Game.State.Data;
using Game.State.Models;

namespace Game.Controllers
{
    public class LoadStartGameStateController
    {
        public LoadStartGameStateController(List<ILoadData<StateData>> services,
            StateDataContainer defaultStateContainer)
        {
            foreach (var service in services) service.LoadFrom(defaultStateContainer.Data);
        }
    }
}