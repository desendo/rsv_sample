using System;
using System.Collections;
using System.Collections.Generic;
using Modules.Common;
using Modules.Reactive.Values;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI
{
    public class ParameterView : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Image _progress;

        private readonly List<IDisposable> _subs = new List<IDisposable>();
        public void Bind(IReadOnlyReactiveVariable<string> title, IReadOnlyReactiveVariable<string> value,
            IReadOnlyReactiveVariable<float> progress)
        {
            title.Subscribe(x => _title.text = x).AddTo(_subs);
            value.Subscribe(x => _value.text = x).AddTo(_subs);
            progress.Subscribe(x => _progress.fillAmount = x).AddTo(_subs);
        }

        public void Dispose()
        {
            _subs?.DisposeAndClear();
        }
    }
}
