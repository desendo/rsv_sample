using System;
using Game.Services;
using Modules.Common;
using UnityEngine;

namespace Game.Controllers
{
    public class MoveHeroByWayPointController
    {
        private readonly HeroService _heroService;
        private readonly IUpdateProvider _updateProvider;
        private IDisposable _moveRoutine;

        public MoveHeroByWayPointController(SignalBus signalBus, HeroService heroService,
            IUpdateProvider updateProvider)
        {
            _heroService = heroService;
            _updateProvider = updateProvider;
            _heroService.Hero.HasWayPoint.Subscribe(HandleHeroHasWayPointChanged);
            _heroService.Hero.WayPoint.Subscribe(HandleWayPointChanged);
        }

        private void HandleWayPointChanged(Vector3 obj)
        {
            if (_heroService.Hero.HasWayPoint.Value)
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
            var target = _heroService.Hero.WayPoint.Value;
            var self = _heroService.Hero.Position.Value;
            var dir = (target - self).normalized;
            var quaternion = Quaternion.LookRotation(dir, Vector3.up);
            _heroService.Hero.Rotation.Value = quaternion;
        }

        //подобные вещи обычно выносятся в утилки,
        //но здесь и далее максимум всего в контроллерах для наглядности и быстроты правки
        private void MoveHeroRoutine(float deltaTime)
        {
            var target = _heroService.Hero.WayPoint.Value;
            var self = _heroService.Hero.Position.Value;

            var delta = target - self;
            var deltaMag = delta.magnitude;

            var dir = delta / deltaMag;

            var stepMag =  _heroService.HeroParameters.MaxMoveSpeed.Value * _heroService.HeroParameters.MoveSpeedFactor.Value * deltaTime;
            var step = dir * stepMag;

            if (deltaMag - stepMag < 0.1f)
                _heroService.Hero.HasWayPoint.Value = false;
            else
                _heroService.Hero.Position.Value += step;
        }
    }
}