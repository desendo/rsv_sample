using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.State.Data
{
    [Serializable]
    public class StateData
    {
        public HeroData HeroData = new();
        public CameraData CameraData = new();
        public List<ItemData> HeroStorageItems = new();
        public List<WorldResourceData> WorldResourcesData = new();
        public List<WorldItemData> WorldItemsData = new();
        public HeroParametersData HeroParametersData = new();

        public static int GenerateUid()
        {
            return (int)(Random.value * int.MaxValue);
        }
    }

    [Serializable]
    public class WorldResourceData
    {
        public string TypeId;
        public string ItemType;
        public Vector3 Position;
        public float Rotation;
        public bool Selected;

        public int Count;
        public int Capacity;
        public float Progress; //

        [HideInInspector] public int UId;
    }

    [Serializable]
    public class WorldItemData
    {
        public string TypeId;
        public Vector3 Position;
        public float Rotation;
        public bool Selected;
        [HideInInspector] public int UId;
    }

    [Serializable]
    public class HeroData
    {
        public Vector3 Position;
        public float Rotation;
        public bool HasWayPoint;
        public Vector3 WayPoint;
        public bool Selected;
        public JobData CurrentJob;
        public string Speech;
    }

    [Serializable]
    public class HeroParametersData
    {
        public float Hunger;
        public float Stamina;
        public float StaminaMax;
        public float HungerMax;
        public float MaxMass;
        public float MaxMoveSpeed;
        public float MoveSpeedFactor;
    }

    [Serializable]
    public class CameraData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 PositionShift;
    }

    [Serializable]
    public class JobData
    {
        public int JobTargetUid;
    }

    [Serializable]
    public class ItemData
    {
        public int UId;
        public string TypeId;
        public float ViewRotation;
        public Vector2 ViewPosition;
        public float Mass;
    }
}