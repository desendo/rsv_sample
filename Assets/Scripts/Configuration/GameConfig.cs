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
        public List<StringId<float>> ItemMasses;
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

        public float GetItemMass(string typeIdValue)
        {
            StringId<float> entry = null;
            foreach (var x in ItemMasses)
            {
                if (x.Id == typeIdValue)
                {
                    entry = x;
                    break;
                }
            }

            if (entry == null)
                return 1;

            return entry.Value;
        }
    }

    [Serializable]
    public class Localization
    {
        [SerializeField] private List<ObjectTitleEntry> _objectTitleEntries = new();


        //linq для читаемости
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