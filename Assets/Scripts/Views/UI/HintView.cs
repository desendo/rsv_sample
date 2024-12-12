using System;
using System.Collections.Generic;
using Game.Services;
using Modules.Common;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HintView : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private RectTransform _panelRect;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _header;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private void Awake()
        {
            var service = Di.Instance.Get<HintService>();
            service.HintText.Subscribe(text => _text.text = text).AddTo(_disposables);
            service.HintHeader.Subscribe(text => _header.text = text).AddTo(_disposables);
            service.HintScreenPosition.Subscribe(text =>
                _panel.transform.position = Input.mousePosition + new Vector3(_panelRect.rect.width/2f, -_panelRect.rect.height/2f, 0f)).AddTo(_disposables);
            service.HintShown.Subscribe(_panel.SetActive).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.DisposeAndClear();
        }
    }
}