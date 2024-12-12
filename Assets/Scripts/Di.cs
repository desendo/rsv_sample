using Game.Rule.Inventory;
using Game.State.Config;
using Game.Rule.Physics;
using Game.Rule.World;
using Game.Rules;
using Game.Rules.Camera;
using Game.Rules.Infrastructure;
using Game.Rules.Map;
using Game.Rules.UI;
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

            //СЛОЙ state (services, models, data, config)
            //представляет собой состояние которое инициализируется извне
            //конвенция - корневые элементы слоя не должны ссылаться друг на друга
            //то есть сервисы не должны резолвить другие сервисы (даже SignalBus и UpdateProvider)
            // models, data, configs, dataAdapters - это вспомогательные классы для сервисов. в общем смысле можно и без них
            Add<SignalBus>();
            Add<UpdateProvider>(new GameObject("UpdateProvider").AddComponent<UpdateProvider>()); //mono ticks
            Add<GameConfig>(Resources.Load<GameConfigDataContainer>(nameof(GameConfigDataContainer)).Data);
            Add<StateDataContainer>(Resources.Load<StateDataContainer>("DefaultStateDataContainer"));

            Add<WorldResourcesService>();
            Add<WorldItemsService>();
            Add<CameraService>();
            Add<UnitsService>();
            Add<UIViewService>();
            Add<HintService>();
            Add<MapService>();
            Add<PhysicsService>();
            Add<GameStateDataService>();

            //СЛОЙ Rules
            //конвенция - корневые элементы слоя не должны ссылаться друг на друга
            //то есть правила не должны резолвить другие правила
            //но могут резолвить сервисы (только их)
            AddInject<LoadGameConfigRule>(); // <----- игра начинается тут (загружаем конфиги)
            AddInject<LoadStartGameStateRule>(); // <----- и тут (загружаем изначальное состояние из StateData
                                                       // который сериализован в ассете DefaultStateDataContainer)

            AddInject<LocalizationLoadRule>();
            AddInject<PauseRule>();
            AddInject<SaveLoadRestartRule>();

            AddInject<SelectWorldViewsRule>();
            AddInject<HoverAndHintWorldViewsRule>();
            AddInject<HandleActionRequestForHeroRule>();
            AddInject<DoHeroJobRule>();
            AddInject<CalculateItemsMassRule>();
            AddInject<MassEffectRule>();
            AddInject<CancelHeroJobRule>();
            AddInject<ShowInventoryRule>();
            AddInject<SetHeroMoveWayPointByGroundClickRule>();
            AddInject<UnityPhysics2dUpdateRule>();
            AddInject<MoveHeroByWayPointRule>();
            AddInject<DropItemsRule>();
            AddInject<ConsumeItemsRule>();
            AddInject<UpdateWorldResourcesRule>();

            AddInject<CameraZoomRule>();
            AddInject<CameraFollowRule>();
            AddInject<CameraRotateRule>();

            AddInject<ActualizeBagItemsRule>();

            AddInject<UpdateMapRule>();
            AddInject<ChangeMapParametersRule>();
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