using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class HintService : IReset
    {
        public IReactiveVariable<Vector3> HintScreenPosition { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<bool> HintShown { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<string> HintText { get; } = new ReactiveVariable<string>();
        public IReactiveVariable<string> HintHeader { get; } = new ReactiveVariable<string>();

        public float HintTimer { get; set; }
        public float HintShowDelay { get; set; }
        public float HintHideDelay { get; set; }
        public bool IsTimeToShow => HintTimer > HintShowDelay;
        public bool IsTimeToHide => HintTimer > HintHideDelay + HintShowDelay;

        public void Reset()
        {
            HintText.Value = "";
            HintShown.Value = false;
            HintTimer = 0f;
        }

        public void AddTime(float time)
        {
            HintTimer += time;
        }
    }
}