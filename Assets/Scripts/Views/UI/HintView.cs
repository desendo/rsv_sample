using System;
using Game.Services;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HintView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _header;
        private Canvas _canvas;
        private CameraService _cameraService;

        private void Awake()
        {
            _cameraService = Di.Instance.Get<CameraService>();
            _canvas = GetComponentInParent<Canvas>();
            var service = Di.Instance.Get<HintService>();
            service.HintText.Subscribe(text => _text.text = text);
            service.HintHeader.Subscribe(text => _header.text = text);
            service.HintShown.Subscribe(_panel.SetActive);
            service.HintScreenPosition.Subscribe(ChangePosition);
        }

        private void ChangePosition(Vector3 screenPoint)
        {
            _panel.transform.position = screenPoint;
        }

    }
}