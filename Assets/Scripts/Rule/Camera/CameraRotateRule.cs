using Game.Services;
using TMPro;
using UnityEngine;
using static UnityEngine.Input;

namespace Game.Rules.Camera
{
    public class CameraRotateRule
    {
        private readonly CameraService _cameraService;
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;

        public CameraRotateRule(CameraService cameraService, GameConfig gameConfig,
            IUpdateProvider updateProvider, HeroService heroService)
        {
            _cameraService = cameraService;
            _gameConfig = gameConfig;
            _heroService = heroService;

            updateProvider.OnTick.Subscribe(Update);
        }

        private float _prevMouseX;
        private void Update(float dt)
        {
            if(!_heroService.Hero.Selected.Value)
                return;

            if (GetMouseButtonDown(2))
            {
                _cameraService.Rotating.Value = true;
            }

            if (_cameraService.Rotating.Value)
            {
                var rotationStep = (mousePosition.x - _prevMouseX) * dt * _gameConfig.CameraRotationSpeed;
                var quaternionStep = Quaternion.AngleAxis(rotationStep, Vector3.up);
                var offset = _cameraService.Position.Value - _heroService.Hero.Position.Value;
                var rotatedOffset = quaternionStep * offset;
                _cameraService.Position.Value = _heroService.Hero.Position.Value + rotatedOffset;
                _cameraService.Rotation.Value = quaternionStep * _cameraService.Rotation.Value;
            }
            if (GetMouseButtonUp(2))
            {
                _cameraService.Rotating.Value = false;
            }

            _prevMouseX = mousePosition.x;
        }

    }
}