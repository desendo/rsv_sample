using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;

namespace Game.Rules
{
    public class HandleActionRequestForHeroRule
    {
        private readonly GameConfig _gameConfig;
        private readonly UnitsService _unitsService;

        public HandleActionRequestForHeroRule(ISignalBus signalBus, UnitsService unitsService,
            GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _unitsService = unitsService;
            signalBus.Subscribe<WorldViewSignals.ActionRequest>(HandleActionRequest);
        }

        private void HandleActionRequest(WorldViewSignals.ActionRequest obj)
        {
            if (!_unitsService.Hero.Selected.Value)
                return;

            if (obj.Model is not IJobModel jobModel)
                return;

            if (obj.Model is not IWorldModel worldModel)
                return;

            var delta = worldModel.Position.Value - _unitsService.Hero.Position.Value;
            if (delta.magnitude > _gameConfig.HeroInteractionDistance)
            {
                var wayPoint = -delta.normalized * _gameConfig.HeroInteractionDistance + worldModel.Position.Value;
                _unitsService.Hero.WayPoint.Value = wayPoint;
                _unitsService.Hero.HasWayPoint.Value = true;
            }

            _unitsService.Hero.CurrentJob.Value = new HeroModel.Job
            {
                JobTargetUid = jobModel.UId
            };
        }
    }
}