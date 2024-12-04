using System.Collections.Generic;
using Game.Configuration;
using Game.Services;
using Game.Signals;
using Game.State.Data;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Controllers
{
    public class SaveLoadRestartController
    {
        public SaveLoadRestartController(List<ILoadData<StateData>> loadServices,
            List<ISaveData<StateData>> saveServices,
            List<IReset> resetServices,
            GameStateDataService gameStateDataService,
            ISignalBus signalBus,
            StateDataContainer defaultStateContainer)
        {
            signalBus.Subscribe<UIViewSignals.RestartGameRequest>(request =>
                {
                    foreach (var resetService in resetServices)
                        resetService.Reset();

                    foreach (var service in loadServices)
                        service.LoadFrom(defaultStateContainer.Data);
                }
            );

            signalBus.Subscribe<UIViewSignals.QuickLoadGameRequest>(request =>
                {
                    var data = gameStateDataService.Load();
                    if (data == null)
                    {
                        Debug.LogWarning("null save data. cancel quickload");
                        return;
                    }

                    foreach (var resetService in resetServices)
                        resetService.Reset();

                    foreach (var service in loadServices)
                        service.LoadFrom(data);
                }
            );

            signalBus.Subscribe<UIViewSignals.QuickSaveGameRequest>(request =>
                {
                    var data = new StateData();
                    foreach (var service in saveServices)
                        service.SaveTo(data);

                    gameStateDataService.Save(data);
                }
            );
        }
    }
}