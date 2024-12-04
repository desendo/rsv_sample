using DG.Tweening;
using Game.Services;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HeroCanvasView : MonoBehaviour
    {
        [SerializeField] private GameObject _speechBubble;
        [SerializeField] private TextMeshProUGUI _speechText;
        private Transform _cameraTransform;
        private GameConfig _gameConfig;
        private HeroService _heroService;
        private Sequence _seq;


        private void Awake()
        {
            _cameraTransform = GetComponent<Canvas>().worldCamera.transform;
            _gameConfig = Di.Instance.Get<GameConfig>();
            _heroService = Di.Instance.Get<HeroService>();
            _heroService.Hero.Speech.Subscribe(OnSpeech);

            _speechBubble.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                _cameraTransform.rotation * Vector3.up);
        }

        private void OnSpeech(string obj)
        {
            _seq?.Kill();
            _speechBubble.SetActive(true);
            _speechText.text = obj;
            _seq = DOTween.Sequence();
            _seq.AppendInterval(_gameConfig.SpeechBubbleTime);
            _seq.AppendCallback(() => _speechBubble.SetActive(false));
        }
    }
}