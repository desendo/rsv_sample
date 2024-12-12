
using Game.Services;
using Game.Signals;
using Game.Views.Common;
using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MiniMapView : MonoBehaviour
    {
        [SerializeField] private Button _minus;
        [SerializeField] private Button _plus;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _panel;
        [SerializeField] private ToggleMapRequestButton _showHide;
        private Texture2D _map;
        private MapService _mapService;
        private GameConfig _gameConfig;
        private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus = Di.Instance.Get<SignalBus>();
            _plus.onClick.AddListener(()=>_signalBus.Fire(new UIViewSignals.ZoomMapRequest(true)));
            _minus.onClick.AddListener(()=>_signalBus.Fire(new UIViewSignals.ZoomMapRequest(false)));
            
            _gameConfig = Di.Instance.Get<GameConfig>();

            _mapService = Di.Instance.Get<MapService>();
            _mapService.Shown.Subscribe(_panel.SetActive);
            _showHide.BindState(_mapService.Shown);
            _map = new Texture2D(_gameConfig.MapResolution, _gameConfig.MapResolution);
            _map.filterMode = FilterMode.Point;

            var sprite = Sprite.Create(_map, new Rect(0, 0, _map.width, _map.height), new Vector2(0.5f, 0.5f));
            _image.sprite = sprite;
        }

        private void Update()
        {
            for (int i = 0; i < _map.width; i++)
            {
                for (int j = 0; j < _map.height; j++)
                {
                    _map.SetPixel(i,j, Color.gray);
                }
            }

            foreach (var (vector2, color) in _mapService.Pixels)
            {
                _map.SetPixel(vector2.x, vector2.y,color);
            }
            _map.Apply();
            
        }
    }
}
