using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class CameraService : ISaveData<StateData>, ILoadData<StateData>, IReset
    {
        public IReactiveVariable<bool> Rotating { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Vector3> PositionShift { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
        public IReactiveVariable<Camera> Camera { get; } = new ReactiveVariable<Camera>();

        public void LoadFrom(in StateData data)
        {
            Position.Value = data.CameraData.Position;
            Rotation.Value = Quaternion.Euler(data.CameraData.Rotation);
            PositionShift.Value = data.CameraData.PositionShift;
        }

        public void Reset()
        {
            Rotating.Value = false;
        }

        public void SaveTo(StateData data)
        {
            data.CameraData.Position = Position.Value;
            data.CameraData.Rotation = Rotation.Value.eulerAngles;
            data.CameraData.PositionShift = PositionShift.Value;
        }

        public void RegisterCamera(Camera camera)
        {
            Camera.Value = camera;
        }
    }
}