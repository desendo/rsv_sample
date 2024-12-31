using System;
using Game.State.Data;
using Game.State.Data.DataAdapters;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class IntegerFillModel : IDataAdapter<IntegerFillModel, IntegerFillData>
    {
        private int _cyclesCompleted;

        public IntegerFillModel()
        {
            OnCycle += RunOnCycle;
            CycleTime.Subscribe(x => UpdateNormalized());
            RunTime.Subscribe(x => UpdateNormalized());
        }

        public ReactiveVariable<int> Max { get; } = new();
        public ReactiveVariable<int> Current { get; } = new();
        public ReactiveVariable<float> CycleTime { get; } = new();
        public ReactiveVariable<float> RunTime { get; } = new();
        public ReactiveVariable<float> NormalizedCycle { get; } = new();

        public bool Started { get; private set; }

        public void ModelToData(in IntegerFillModel model, in IntegerFillData data)
        {
            data.Max = model.Max.Value;
            data.Current = model.Current.Value;
            data.CycleTime = model.CycleTime.Value;
            data.Runtime = model.RunTime.Value;
            data.Started = model.Started;
        }


        public void DataToModel(in IntegerFillData data, in IntegerFillModel model)
        {
            model.Max.Value = data.Max;
            model.Current.Value = data.Current;
            model.RunTime.Value = data.Runtime;
            model.CycleTime.Value = data.CycleTime;
            model.SetStarted(data.Started);
        }

        public event Action OnCycle;

        private void UpdateNormalized()
        {
            NormalizedCycle.Value = Mathf.Clamp01(RunTime.Value / CycleTime.Value);
        }

        private void RunOnCycle()
        {
            if (Max.Value > Current.Value)
                Current.Value++;
        }

        private void Start()
        {
            if (CycleTime.Value <= 0f)
                throw new ArgumentOutOfRangeException();

            Started = true;
        }

        private void Stop()
        {
            _cyclesCompleted = 0;
            RunTime.Value = 0;
            Started = false;
        }

        public void Update(float dt)
        {
            if (!Started)
                return;

            var runtime = RunTime.Value;
            runtime += dt;
            var totalCycles = Mathf.FloorToInt(runtime / CycleTime.Value);

            if (totalCycles <= _cyclesCompleted)
            {
                RunTime.Value = runtime;
                return;
            }

            var newCyclesCount = totalCycles - _cyclesCompleted;

            for (var i = 0; i < newCyclesCount; i++)
                OnCycle?.Invoke();

            _cyclesCompleted += newCyclesCount;

            if (_cyclesCompleted > 0)
            {
                runtime -= _cyclesCompleted * CycleTime.Value;
                _cyclesCompleted = 0;
            }

            RunTime.Value = runtime;
        }

        private void SetStarted(bool dataStarted)
        {
            if (dataStarted)
                Start();
            else
                Stop();
        }
    }
}