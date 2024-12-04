using DG.Tweening;
using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public sealed class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private readonly ReactiveEvent<float> _fixedTick = new();
        private readonly ReactiveEvent<float> _lateTick = new();
        private readonly IReactiveVariable<bool> _paused = new ReactiveVariable<bool>();
        private readonly ReactiveEvent<float> _tick = new();

        private void Awake()
        {
            Physics2D.simulationMode = SimulationMode2D.Script;
        }

        private void Update()
        {
            if (_paused.Value)
                return;

            _tick.Invoke(Time.deltaTime);
            DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            if (_paused.Value)
                return;

            Physics2D.Simulate(Time.fixedDeltaTime);
            _fixedTick.Invoke(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _lateTick.Invoke(Time.deltaTime);
        }

        public IReadOnlyReactiveEvent<float> OnTick => _tick;
        public IReadOnlyReactiveEvent<float> OnFixedTick => _fixedTick;
        public IReadOnlyReactiveEvent<float> OnLateTick => _lateTick;
        public IReadOnlyReactiveVariable<bool> Paused => _paused;

        public void SetPaused(bool paused)
        {
            _paused.Value = paused;
        }
    }

    public interface IUpdateProvider
    {
        public IReadOnlyReactiveEvent<float> OnTick { get; }
        public IReadOnlyReactiveEvent<float> OnFixedTick { get; }
        public IReadOnlyReactiveEvent<float> OnLateTick { get; }
        public IReadOnlyReactiveVariable<bool> Paused { get; }
        public void SetPaused(bool paused);
    }
}