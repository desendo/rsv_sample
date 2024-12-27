using System;
using Game.Services;
using Game.Signals;
using Modules.Common;
using UnityEngine.EventSystems;

namespace Game.Rules
{
    public class SetHeroMoveWayPointByGroundClickRule
    {
        private readonly HeroService _heroService;
        private IDisposable _moveRoutine;

        public SetHeroMoveWayPointByGroundClickRule(ISignalBus signalBus, HeroService heroService)
        {
            _heroService = heroService;
            signalBus.Subscribe<WorldViewSignals.GroundClick>(HandleGroundClick);
        }

        private void HandleGroundClick(WorldViewSignals.GroundClick obj)
        {
            if (obj.Button != PointerEventData.InputButton.Right)
                return;

            if (!_heroService.Hero.Selected.Value)
                return;

            _heroService.Hero.WayPoint.Value = obj.Position;
            _heroService.Hero.HasWayPoint.Value = true;
        }
    }
}