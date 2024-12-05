using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class HeroModel : ISelectableModel, IModel
    {
        public IReactiveVariable<string> Speech { get; } = new ReactiveVariable<string>();

        public IReactiveVariable<Job> CurrentJob { get; } = new ReactiveVariable<Job>();

        //movement
        public IReactiveVariable<bool> HasWayPoint { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<Vector3> WayPoint { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
        public int UId => 1;

        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>("hero");

        //selection
        public IReactiveVariable<bool> Selected { get; } = new ReactiveVariable<bool>();

        public IReactiveVariable<bool> Hovered { get; } = new ReactiveVariable<bool>();
        public IReactiveEvent OnAction { get; } = new ReactiveEvent();


        public void Say(string speech)
        {
            if (Speech.Value.Length > 100)
            {
                Speech.Value = Speech.Value.Substring(Speech.Value.Length - 100, 100);
            }

            Speech.Value += $">{speech}\n";
        }

        public class Job
        {
            public int JobTargetUid;
        }

    }
}