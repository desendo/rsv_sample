using Game.State.Enum;
using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class HeroModel : ISelectableModel, IModel
    {
        public IReactiveEvent<string> Speech { get; } = new ReactiveEvent<string>();

        public IReactiveVariable<Job> CurrentJob { get; } = new ReactiveVariable<Job>();

        //movement
        public IReactiveVariable<bool> HasWayPoint { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<Vector3> WayPoint { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
        public IReactiveVariable<float> MoveSpeed { get; } = new ReactiveVariable<float>();
        public int UId => 1;

        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>("hero");

        //selection
        public IReactiveVariable<bool> Selected { get; } = new ReactiveVariable<bool>();

        public IReactiveVariable<bool> Hovered { get; } = new ReactiveVariable<bool>();


        public void Say(string speech)
        {
            Speech.Invoke(speech);
        }


        public class Job
        {
            public JobEnum JobId;
            public int JobTargetUid;
        }
    }
}