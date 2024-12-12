using System;
using Game.Services;
using Game.Signals;
using Modules.Common;
using UnityEngine.EventSystems;

namespace Game.Rules
{
    public class SetHeroMoveWayPointByGroundClickRule
    {
        private readonly UnitsService _unitsService;
        private IDisposable _moveRoutine;

        public SetHeroMoveWayPointByGroundClickRule(ISignalBus signalBus, UnitsService unitsService)
        {
            _unitsService = unitsService;
            signalBus.Subscribe<WorldViewSignals.GroundClick>(HandleGroundClick);
        }

        private void HandleGroundClick(WorldViewSignals.GroundClick obj)
        {
            if (obj.Button != PointerEventData.InputButton.Right)
                return;

            if (!_unitsService.Hero.Selected.Value)
                return;

            _unitsService.Hero.WayPoint.Value = obj.Position;
            _unitsService.Hero.HasWayPoint.Value = true;
        }
    }
}