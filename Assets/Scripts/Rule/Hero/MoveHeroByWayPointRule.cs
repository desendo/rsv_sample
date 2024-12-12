using System;
using Game.Services;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class MoveHeroByWayPointRule
    {
        private readonly UnitsService _unitsService;
        private readonly IUpdateProvider _updateProvider;
        private IDisposable _moveRoutine;

        public MoveHeroByWayPointRule(SignalBus signalBus, UnitsService unitsService,
            IUpdateProvider updateProvider)
        {
            _unitsService = unitsService;
            _updateProvider = updateProvider;
            _unitsService.Hero.HasWayPoint.Subscribe(HandleHeroHasWayPointChanged);
            _unitsService.Hero.WayPoint.Subscribe(HandleWayPointChanged);
        }

        private void HandleWayPointChanged(Vector3 obj)
        {
            if (_unitsService.Hero.HasWayPoint.Value)
                RotateToWayPoint();
        }

        private void HandleHeroHasWayPointChanged(bool hasWayPoint)
        {
            if (hasWayPoint)
            {
                RotateToWayPoint();
                //каждый апдейт мы проводим процедуру попытки перемещения героя
                //причем этот апдейт крутится (и даже вызывается) только тогда когда нужно
                _moveRoutine = _updateProvider.OnTick.Subscribe(MoveHeroRoutine);
            }
            else
            {
                _moveRoutine?.Dispose();
            }
        }

        private void RotateToWayPoint()
        {
            var target = _unitsService.Hero.WayPoint.Value;
            var self = _unitsService.Hero.Position.Value;
            var dir = (target - self).normalized;
            var quaternion = Quaternion.LookRotation(dir, Vector3.up);
            _unitsService.Hero.Rotation.Value = quaternion;
        }

        //подобные вещи обычно выносятся в утилки,
        //но здесь и далее максимум всего в контроллерах для наглядности и быстроты правки
        private void MoveHeroRoutine(float deltaTime)
        {
            var target = _unitsService.Hero.WayPoint.Value;
            var self = _unitsService.Hero.Position.Value;

            var delta = target - self;
            var deltaMag = delta.magnitude;

            var dir = delta / deltaMag;

            var stepMag =  _unitsService.HeroParameters.MaxMoveSpeed.Value * _unitsService.HeroParameters.MoveSpeedFactor.Value * deltaTime;
            var step = dir * stepMag;

            if (deltaMag - stepMag < 0.1f)
                _unitsService.Hero.HasWayPoint.Value = false;
            else
                _unitsService.Hero.Position.Value += step;
        }
    }
}