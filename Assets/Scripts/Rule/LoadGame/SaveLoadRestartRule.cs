using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Config;
using Game.State.Data;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class SaveLoadRestartRule
    {
        public SaveLoadRestartRule(List<ILoadData<StateData>> loadServices,
            List<ISaveData<StateData>> saveServices,
            List<IReset> resetServices,
            GameStateDataService gameStateDataService,
            ISignalBus signalBus,
            StateDataContainer defaultStateContainer)
        {
            signalBus.Subscribe<UIViewSignals.RestartGameRequest>(request =>
                {
                    //ресетим состояние, которое нужно ресетить (к примеру у них нет методов LoadData)
                    foreach (var resetService in resetServices)
                        resetService.Reset();

                    //инициализируем состояние данными из сохранения
                    foreach (var service in loadServices)
                        service.LoadFrom(defaultStateContainer.Data);
                }
            );

            signalBus.Subscribe<UIViewSignals.QuickLoadGameRequest>(request =>
                {
                    //вытаскиваем сохраненку через сервис, а если игрок до этого не сохранялся (сохраненка пустая), то отменяем процедуру
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
                    //создаем новый инстанс StateData
                    var data = new StateData();
                    //прогоняем его по всем сервисам, каждый туда кладет что-то свое, чтобы сберечь) 
                    foreach (var service in saveServices)
                        service.SaveTo(data);

                    //передаем наполненый StateData в надежные руки сервиса сохранения. он его положит в гипернадежные PlayerPrefs
                    gameStateDataService.Save(data);
                }
            );
        }
    }
}