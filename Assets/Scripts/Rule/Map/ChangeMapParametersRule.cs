using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rules.Map
{
    public class ChangeMapParametersRule
    {
        private readonly GameConfig _gameConfig;
        private readonly MapService _mapService;


        public ChangeMapParametersRule(MapService mapService, GameConfig gameConfig, SignalBus signalBus)
        {
            _mapService = mapService;
            _gameConfig = gameConfig;

            signalBus.Subscribe<UIViewSignals.ZoomMapRequest>(HandleZoomMapRequests);
            _mapService.MapDistance.Subscribe(currentDistance =>
            {
                if (currentDistance > _gameConfig.MapDistanceMax)
                    _mapService.MapDistance.Value = _gameConfig.MapDistanceMax;
                if (currentDistance < _gameConfig.MapDistanceMin)
                    _mapService.MapDistance.Value = _gameConfig.MapDistanceMin;
            });
        }

        private void HandleZoomMapRequests(UIViewSignals.ZoomMapRequest obj)
        {
            var targetValue = _mapService.MapDistance.Value;
            if (obj.Plus)
                targetValue -= _gameConfig.MapDistanceStep;
            else
                targetValue += _gameConfig.MapDistanceStep;

            if (targetValue > _gameConfig.MapDistanceMax)
                targetValue = _gameConfig.MapDistanceMax;
            if (targetValue < _gameConfig.MapDistanceMin)
                targetValue = _gameConfig.MapDistanceMin;

            _mapService.MapDistance.Value = targetValue;
        }
    }
}