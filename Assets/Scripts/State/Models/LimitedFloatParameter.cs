using System;
using System.Collections.Generic;
using Modules.Common;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class LimitedFloatParameter : IDisposable
    {
        private readonly IReactiveVariable<float> _current = new ReactiveVariable<float>(0);
        private readonly IReactiveVariable<float> _max = new ReactiveVariable<float>(1);
        private readonly IReactiveVariable<float> _normalized = new ReactiveVariable<float>();
        private readonly IReactiveVariable<float> _normalizedUnclamped = new ReactiveVariable<float>();
        private readonly List<IDisposable> _subs = new();
        private readonly IReactiveVariable<string> _title = new ReactiveVariable<string>();
        private readonly IReactiveVariable<string> _valueString = new ReactiveVariable<string>();

        public LimitedFloatParameter()
        {
            Current.Subscribe(x => { UpdateValues(); });
            Max.Subscribe(x => { UpdateValues(); });
        }

        public IReadOnlyReactiveVariable<string> Title => _title;

        public IReadOnlyReactiveVariable<string> ValueString => _valueString;

        public IReadOnlyReactiveVariable<float> Normalized => _normalized;
        public IReadOnlyReactiveVariable<float> NormalizedUnclamped => _normalizedUnclamped;

        public IReadOnlyReactiveVariable<float> Current => _current;

        public IReadOnlyReactiveVariable<float> Max => _max;

        public void Dispose()
        {
            _subs.DisposeAndClear();
        }

        private void UpdateValues()
        {
            _normalized.Value = Mathf.Clamp01(Current.Value / Max.Value);
            _normalizedUnclamped.Value = Current.Value / Max.Value;
            _valueString.Value = $"{Current.Value:F1}/{Max.Value:F1}";
        }

        public void BindTitle(IReactiveVariable<string> title)
        {
            title.Subscribe(x => _title.Value = x).AddTo(_subs);
        }

        public void SetCurrent(float value)
        {
            _current.Value = value;
        }

        public void SetMax(float value)
        {
            _max.Value = value;
        }

        public void AddCurrent(float addValue, bool limit = false)
        {
            var target = _current.Value + addValue;
            if (limit)
                target = Mathf.Clamp(target, 0, _max.Value);
            SetCurrent(target);
        }
    }
}