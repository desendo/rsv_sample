using Game.Services;
using UnityEngine;

namespace Game.Controllers.Camera
{
    public class CameraFollowController
    {
        private readonly CameraService _cameraService;
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;
        
        public CameraFollowController(CameraService cameraService, GameConfig gameConfig,
            IUpdateProvider updateProvider, HeroService heroService)
        {
            _cameraService = cameraService;
            _gameConfig = gameConfig;
            _heroService = heroService;

            updateProvider.OnTick.Subscribe(Update);
        }

        private void Update(float dt)
        {
            if(!_heroService.Hero.Selected.Value)
                return;

            var ray = _cameraService.Camera.Value.ViewportPointToRay(Vector2.one * 0.5f);

            var planeDifference = Vector3.zero;
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var dist))
            {
                var cameraCenterPointOnPlane = ray.GetPoint(dist);
                planeDifference = _heroService.Hero.Position.Value - cameraCenterPointOnPlane;
            }

            if(_gameConfig.FollowToleranceSq > planeDifference.sqrMagnitude)
                return;
            
            _cameraService.Position.Value = Vector3.Lerp(_cameraService.Position.Value, _cameraService.Position.Value + planeDifference, dt * _gameConfig.FollowSpeed);

        }

    }
}