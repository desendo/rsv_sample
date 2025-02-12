﻿using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class WorldItemModel : ISelectableModel, IWorldModel, IInteractionModel
    {
        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>();
        public int UId { get; set; }

        public IReactiveVariable<bool> Selected { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<bool> Hovered { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
    }
}