using System;
using System.Collections.Generic;
using System.Linq;
using Game.Views.Environment.Items;
using Game.Views.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public class GameConfig
    {
        [Header("Prefabs")]
        public List<PrefabEntry<WorldResourceView>> ResourcePrefabEntries;
        public List<PrefabEntry<WorldItemView>> WorldItemsPrefabEntries;
        public List<PrefabEntry<BagItemView>> BagItemViewPrefabEntries;
        public List<StringId<List<Result>>> ItemActionsResult;
        public List<MassSizeEntry> ItemParameters;
        [Header("Strings")]
        public Localization Localization;
        [Header("Common parameters")]
        public float HeroInteractionDistance;
        public float SpeechBubbleTime;
        public float CameraZoomMinHeight;
        public float CameraZoomMaxHeight;
        public float CameraZoomStep;
        public float CameraFollowToleranceSq;
        public float CameraFollowSpeed;
        public float CameraRotationSpeed = 1f;
        public float HintShowDelay = 0.5f;
        public float HintHideDelay = 1.5f;
        public float CameraFollowShiftGrowSpeed;
        public float CameraFollowShiftMagnitude;

        public float InventoryScale;
        public float MapDistanceMax;
        public float MapDistanceStep;
        public float MapDistanceMin;
        public int MapResolution;
        public float Gravity2d = -10;

        public MassSizeEntry GetItemParam(string id)
        {
            foreach (var x in ItemParameters)
            {
                if (x.Id == id) return x;
            }

            return null;
        }
    }

    [Serializable]
    public class Localization
    {
        [SerializeField] private List<ObjectTitleEntry> _objectTitleEntries = new();
        //wip
        [SerializeField] public List<Language> Languages = new();

        public string GetObjectProduct(string id)
        {
            return _objectTitleEntries.FirstOrDefault(x => x.Id == id)?.Product;
        }

        public string GetObjectDescription(string id)
        {
            return _objectTitleEntries.FirstOrDefault(x => x.Id == id)?.Description;
        }

        public string GetObjectAction(string id)
        {
            return _objectTitleEntries.FirstOrDefault(x => x.Id == id)?.Action;
        }

        public string GetObjectTitle(string id)
        {
            return _objectTitleEntries.FirstOrDefault(x => x.Id == id)?.Title;
        }


        [Serializable]
        public class ObjectTitleEntry
        {
            public string Id;
            public string Title;
            public string Action;
            public string Description;
            public string Product;
        }
        [Serializable]
        public class IdString
        {
            public string Id;
            [FormerlySerializedAs("String")] public string Value;
        }
        [Serializable]
        public class Language
        {
            public SystemLanguage Id;
            public List<IdString> Strings;
        }
        [Serializable]
        public class CommonModelEntry
        {
            public string Id;
            public string Title;
            public string Description;
        }
        [Serializable]
        public class JobEntry
        {
            public string Title;
        }

    }
    [Serializable]
    public class PrefabEntry<T> where T : Component
    {
        public string Id;
        public T Prefab;
    }

    [Serializable]
    public class MassSizeEntry
    {
        public string Id;
        public float Mass;
        public float Size;
    }
    [Serializable]
    public class StringId<T>
    {
        public string Id;
        public T Value;
    }

    public enum ActionType
    {
        Consume,

    }
    public enum InstantEffectType
    {
        ReduceHunger,

    }
    [Serializable]
    public class Result
    {
        public ActionType ActionType;
        public List<InstantEffect> InstantEffects;
    }
    [Serializable]
    public class InstantEffect
    {
        public InstantEffectType InstantEffectType;
        public int Value;
    }
}