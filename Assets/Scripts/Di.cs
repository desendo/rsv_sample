using Game.Configuration;
using Game.Controllers;
using Game.Controllers.Camera;
using Game.Controllers.Infrastructure;
using Game.Controllers.UI;
using Game.Services;
using Modules.Common;
using Modules.DependencyInjection;
using UnityEngine;

namespace Game
{
    public class Di : DependencyContainer
    {
        private void InstallDependencies()
        {
            //infrastructure
            Add<SignalBus>();
            Add<UpdateProvider>(new GameObject("UpdateProvider").AddComponent<UpdateProvider>()); //mono ticks

            //default data and config
            Add<GameConfig>(Resources.Load<GameConfigDataContainer>(nameof(GameConfigDataContainer)).Data);
            Add<StateDataContainer>(Resources.Load<StateDataContainer>("DefaultStateDataContainer"));

            //state (services and models)
            Add<WorldResourcesService>();
            Add<WorldItemsService>();
            Add<CameraService>();
            Add<HeroService>();
            Add<UIViewService>();
            Add<HintService>();
            Add<GameStateDataService>();

            //controllers
            AddInject<LoadGameConfigController>(); // <----- игра начинается тут (загружаем конфиги)
            AddInject<LoadStartGameStateController>(); // <----- и тут (загружаем изначальное состояние из SO)

            AddInject<PauseController>();
            AddInject<SaveLoadRestartController>();

            AddInject<SelectWorldViewsController>();
            AddInject<HandleActionRequestForHeroController>();
            AddInject<DoHeroJobController>();
            AddInject<SetHeroMoveWayPointByGroundClickController>();
            AddInject<MoveHeroByWayPointController>();
            AddInject<DropItemsController>();

            AddInject<CameraFollowController>();
            AddInject<CameraRotateController>();

            AddInject<SimplePopupController>();
            AddInject<ActualizeBagItemsController>();
        }

        #region Singleton

        private static Di _instance;

        public static Di Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Di();
                    _instance.InstallDependencies();
                }

                return _instance;
            }
        }

        #endregion
    }
}