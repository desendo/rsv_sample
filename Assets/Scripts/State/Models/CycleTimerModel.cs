using System;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class CycleTimerModel
    {
        private float _cycleTime;
        private ReactiveVariable<float> _runTime;
        private int _cyclesCompleted;

        public bool Started { get; private set; }

        public IReadOnlyReactiveVariable<float> Runtime => _runTime;

        public event Action OnCycle;


        public void Init(float cycleTime)
        {
            if (cycleTime <= 0f)
                throw new ArgumentOutOfRangeException();

            _cycleTime = cycleTime;
        }

        public void Start()
        {
            if(_cycleTime<= 0f)
                throw new ArgumentOutOfRangeException();

            Started = true;
        }

        public void Stop()
        {
            _cyclesCompleted = 0;
            _runTime.Value = 0;
            Started = false;
        }

        //как показали тесты такая логика расчета дает точные результаты с минимумом кода
        public void Update(float dt)
        {
            if(!Started)
                return;

            var runtime = _runTime.Value;
            runtime += dt;
            var totalCycles = Mathf.FloorToInt(runtime / _cycleTime);

            if (totalCycles <= _cyclesCompleted) return;

            var newCyclesCount = totalCycles - _cyclesCompleted;

            for (var i = 0; i < newCyclesCount; i++)
                OnCycle?.Invoke();

            _cyclesCompleted += newCyclesCount;

            if (_cyclesCompleted > 0)
            {
                runtime -= _cyclesCompleted * dt;
                _cyclesCompleted = 0;
            }

            _runTime.Value = runtime;
        }
    }
}