using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules.Map
{
    public class UpdateMapRule
    {
        private readonly List<IModelEnum<IWorldModel>> _worldModelsLists;
        private readonly MapService _mapService;
        private readonly HeroService _heroService;
        private readonly GameConfig _gameConfig;
        private readonly CameraService _cameraService;


        public UpdateMapRule(List<IModelEnum<IWorldModel>> worldModelsLists, IUpdateProvider updateProvider,
            SignalBus signalBus,
            MapService mapService, HeroService heroService, GameConfig gameConfig, CameraService cameraService)
        {
            _worldModelsLists = worldModelsLists;
            _mapService = mapService;
            _heroService = heroService;
            _gameConfig = gameConfig;
            _cameraService = cameraService;
            signalBus.Subscribe<UIViewSignals.ToggleMapShownRequest>(x =>
                _mapService.Shown.Value = !_mapService.Shown.Value);

            updateProvider.OnTick.Subscribe(Update);
        }

        private void Update(float obj)
        {
            _mapService.Pixels.Clear();
            var mapWidthPixels = _gameConfig.MapResolution;
            var center = _heroService.Hero.Position.Value;
            var mapDistance = _mapService.MapDistance.Value;
            var mapShift = Vector3.one * ( mapDistance * 0.5f);

            var cameraPlaneRotation = ProjectRotationOnPlane(_cameraService.Rotation.Value, Vector3.up);
            foreach (var worldModelsList in _worldModelsLists)
            {
                foreach (var worldModel in worldModelsList.GetEnum())
                {
                    var delta = worldModel.Position.Value - center;
                    delta = cameraPlaneRotation * delta;
                    if(delta.x > mapDistance* 0.5f)
                        continue;
                    if(delta.z > mapDistance* 0.5f)
                        continue;
                    if(delta.x < - mapDistance* 0.5f)
                        continue;
                    if(delta.z < -mapDistance* 0.5f)
                        continue;

                    var relativePosition = (delta + mapShift) / mapDistance;

                    var pixelCoordinates = new Vector2Int((int)(relativePosition.x  * mapWidthPixels),
                        (int)(relativePosition.z  * mapWidthPixels ));


                    var imageColor = Color.red;
                    if (worldModel is WorldResourceModel)
                    {
                        imageColor = Color.yellow;
                    }
                    if (worldModel is HeroModel)
                    {
                        imageColor = Color.blue;
                    }
                    _mapService.Pixels.Add(new (pixelCoordinates, imageColor));

                }
            }

        }

        private Quaternion ProjectRotationOnPlane(Quaternion originalRotation, Vector3 planeNormal)
        {

            Vector3 forward = originalRotation * Vector3.forward;
            Vector3 forwardOnPlane = Vector3.ProjectOnPlane(forward, planeNormal).normalized;
            return Quaternion.FromToRotation(forwardOnPlane, Vector3.forward);
        }
    }
}