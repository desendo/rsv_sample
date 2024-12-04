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
            if (_heroService.Hero.HasWayPoint.Value) RotateToWayPoint();
        }

        private void HandleHeroHasWayPointChanged(bool hasWayPoint)
        {
            if (hasWayPoint)
            {
                RotateToWayPoint();
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

        private void MoveHeroRoutine(float deltaTime)
        {
            var target = _heroService.Hero.WayPoint.Value;
            var self = _heroService.Hero.Position.Value;

            var delta = target - self;
            var deltaMag = delta.magnitude;
            if (deltaMag < 0.1f) _heroService.Hero.HasWayPoint.Value = false;

            var dir = delta / deltaMag;

            var step = dir * (_heroService.Hero.MoveSpeed.Value * deltaTime);
            _heroService.Hero.Position.Value += step;
        }
    }
}