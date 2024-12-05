using Game.Services;
using UnityEngine;

namespace Game.Controllers.Camera
{
    public class CameraZoomController
    {
        private readonly CameraService _cameraService;
        private readonly GameConfig _gameConfig;

        public CameraZoomController(CameraService cameraService, GameConfig gameConfig,
            IUpdateProvider updateProvider)
        {
            _cameraService = cameraService;
            _gameConfig = gameConfig;

            updateProvider.OnTick.Subscribe(Update);
        }

        private void Update(float obj)
        {
            var delta = Input.mouseScrollDelta.y;
            var shiftDelta = _cameraService.Rotation.Value * Vector3.forward * (_gameConfig.CameraZoomStep * delta);
            var targetValue = _cameraService.Position.Value + shiftDelta;
            if(targetValue.y > _gameConfig.CameraZoomMaxHeight )
                return;
            if(targetValue.y  < _gameConfig.CameraZoomMinHeight )
                return;

            _cameraService.Position.Value = targetValue;

        }
    }
}