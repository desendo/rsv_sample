using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Game.State.Data
{
    [Serializable]
    public class StateData
    {
        public SystemLanguage CurrentLanguage;
        public HeroData HeroData = new();
        public CameraData CameraData = new();
        public StorageData HeroStorageItemsData = new();
        public List<WorldResourceData> WorldResourcesData = new();
        public List<WorldItemData> WorldItemsData = new();
        public HeroParametersData HeroParametersData = new();
        public Map Map = new();
        public PhysicsData Physics = new();

        public static int GenerateUid()
        {
            var uid = 0;
            var buffer = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            uid = BitConverter.ToInt32(buffer, 0) & int.MaxValue;

            return uid;
        }
    }

    [Serializable]
    public class StorageData
    {
        public List<ItemData> Items = new();
        public float Width;
        public float Height;
    }

    [Serializable]
    public class PhysicsData
    {
        public bool Is2dEnabled;
        public float PhysicsGMultiplier;
    }

    [Serializable]
    public class WorldResourceData
    {
        public string TypeId;
        public string ItemType;
        public Vector3 Position;
        public float Rotation;
        public bool Selected;
        public IntegerFillData ResourcesData = new();


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
        public bool InventoryShown;
    }

    [Serializable]
    public class HeroParametersData
    {
        public float Hunger;
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
    }

    [Serializable]
    public class Map
    {
        public float CurrentDistance;
        public bool Shown;
    }
}