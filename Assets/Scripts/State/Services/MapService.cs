using System.Collections.Generic;
using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class MapService : ISaveData<StateData>, ILoadData<StateData>

    {
        public List<(Vector2Int, Color)> Pixels { get; } = new();
        public ReactiveVariable<float> MapDistance { get; } = new();
        public ReactiveVariable<bool> Shown { get; set; } = new();

        public void LoadFrom(in StateData data)
        {
            MapDistance.Value = data.Map.CurrentDistance;
            Shown.Value = data.Map.Shown;
        }

        public void SaveTo(StateData data)
        {
            data.Map.CurrentDistance = MapDistance.Value;
            data.Map.Shown = Shown.Value;
        }
    }
}