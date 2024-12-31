using System;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Views.Common
{
    public class ToggleRequestButton<T> : SignalButton<T>, IDisposable where T : new()
    {
        [SerializeField] protected GameObject _stateIcon;
        [SerializeField] protected bool _inverse;
        private IDisposable _sub;

        public void Dispose()
        {
            _sub?.Dispose();
        }

        public void BindState(IReactiveVariable<bool> state)
        {
            _sub = state.Subscribe(x =>
            {
                if (_inverse)
                    x = !x;
                _stateIcon.transform.localScale = new Vector3(x ? -1 : 1, 1, 1);
            });
        }
    }
}