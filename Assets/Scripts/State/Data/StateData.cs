using System;
using System.Collections.Generic;
using Game.State.Enum;
using UnityEngine;
using UnityEngine.Serialization;
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

        public float MoveSpeed;
        public float Stamina;
        public float StaminaMax;
        public float StaminaRestorationSpeed;
        public JobData CurrentJob;
    }

    [Serializable]
    public class CameraData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    [Serializable]
    public class JobData
    {
        public JobEnum JobId;
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
}