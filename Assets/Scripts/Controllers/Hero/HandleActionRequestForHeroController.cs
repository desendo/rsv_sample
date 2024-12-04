using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;

namespace Game.Controllers
{
    public class HandleActionRequestForHeroController
    {
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;

        public HandleActionRequestForHeroController(ISignalBus signalBus, HeroService heroService,
            
            GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _heroService = heroService;
            signalBus.Subscribe<WorldViewSignals.ActionRequest>(HandleActionRequest);
        }

        private void HandleActionRequest(WorldViewSignals.ActionRequest obj)
        {
            if (!_heroService.Hero.Selected.Value)
                return;

            if (obj.Model is not IJobModel jobModel)
                return;

            if (obj.Model is not IWorldModel worldModel)
                return;

            var delta = worldModel.Position.Value - _heroService.Hero.Position.Value;
            if (delta.magnitude > _gameConfig.InteractionDistance * 1.2f)
            {
                _heroService.Hero.Say(_gameConfig.Localization.GetJobTitle(jobModel.TargetJob));
                
                var wayPoint = -delta.normalized * _gameConfig.InteractionDistance + worldModel.Position.Value;
                _heroService.Hero.WayPoint.Value = wayPoint;
                _heroService.Hero.HasWayPoint.Value = true;
            }

            _heroService.Hero.CurrentJob.Value = new HeroModel.Job
            {
                JobId = jobModel.TargetJob,
                JobTargetUid = jobModel.UId
            };
        }
    }
}