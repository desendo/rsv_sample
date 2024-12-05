using System.Collections.Generic;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class HeroParameters
    {
        public IReactiveVariable<float> MaxMass { get; } = new ReactiveVariable<float>(1);
        public IReactiveVariable<float> MaxMoveSpeed { get; } = new ReactiveVariable<float>(1);
        public IReactiveVariable<float> MoveSpeedFactor { get; } = new ReactiveVariable<float>(1);
        public Parameter Hunger { get; } = new Parameter {Title = { Value = "голод"}};//todo привязать к строкам

    }

    public class Parameter
    {
        public IReactiveVariable<string> Title { get; } = new ReactiveVariable<string>();
        public IReactiveVariable<string> ValueString { get; } = new ReactiveVariable<string>();
        public IReactiveVariable<float> Normalized { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<float> Current { get; } = new ReactiveVariable<float>(0);
        public IReactiveVariable<float> Max { get; } = new ReactiveVariable<float>(1);

        public Parameter()
        {
            Current.Subscribe(x =>
            {
                Normalized.Value = Mathf.Clamp01(Current.Value / Max.Value);
                ValueString.Value = $"{Current.Value}/{Max.Value}";
            });
            Max.Subscribe(x =>
            {
                Normalized.Value = Mathf.Clamp01(Current.Value / Max.Value);
                ValueString.Value = $"{Current.Value}/{Max.Value}";
            });
        }
    }
}