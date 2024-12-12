using System;
using Game.Services;
using UnityEngine;

namespace Game.Rules.Camera
{
    public class CameraFollowRule
    {
        private readonly CameraService _cameraService;
        private readonly GameConfig _gameConfig;
        private readonly IUpdateProvider _updateProvider;
        private readonly UnitsService _unitsService;
        private IDisposable _updateFollowShiftRoutine;


        public CameraFollowRule(CameraService cameraService, GameConfig gameConfig,
            IUpdateProvider updateProvider, UnitsService unitsService)
        {
            _cameraService = cameraService;
            _gameConfig = gameConfig;
            _updateProvider = updateProvider;
            _unitsService = unitsService;
            _unitsService.Hero.HasWayPoint.Subscribe(HandleHeroIsMoving);
            updateProvider.OnTick.Subscribe(Update);
        }

        private void HandleHeroIsMoving(bool isMoving)
        {
            _updateFollowShiftRoutine?.Dispose();
            if (!isMoving)
            {
                _cameraService.PositionShift.Value = Vector3.zero;
            }
            else
            {
                _updateFollowShiftRoutine = _updateProvider.OnTick.Subscribe(UpdateFollowShiftRoutine);

            }
        }

        private void UpdateFollowShiftRoutine(float dt)
        {
            var dir = (_unitsService.Hero.WayPoint.Value - _unitsService.Hero.Position.Value).normalized;
            _cameraService.PositionShift.Value = Vector3.Lerp(_cameraService.PositionShift.Value, dir * _gameConfig.CameraFollowShiftMagnitude, _gameConfig.CameraFollowShiftGrowSpeed * dt);
        }

        private void Update(float dt)
        {
            if(!_unitsService.Hero.Selected.Value)
                return;

            var followPoint = _cameraService.PositionShift.Value + _unitsService.Hero.Position.Value;
            var ray = _cameraService.Camera.Value.ViewportPointToRay(Vector2.one * 0.5f);

            var planeDifference = Vector3.zero;
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var dist))
            {
                var cameraCenterPointOnPlane = ray.GetPoint(dist);
                planeDifference = followPoint - cameraCenterPointOnPlane;
            }

            if (_gameConfig.CameraFollowToleranceSq > planeDifference.sqrMagnitude)
            {
                _cameraService.PositionShift.Value = Vector3.zero;
                return;
            }

            _cameraService.Position.Value = Vector3.Lerp(_cameraService.Position.Value, _cameraService.Position.Value + planeDifference, dt * _gameConfig.CameraFollowSpeed);

        }

    }
}