using System;
using System.Collections.Generic;
using System.Linq;
using Game.State.Enum;
using Game.Views.Environment.Items;
using Game.Views.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public class GameConfig
    {
        public List<PrefabEntry<WorldResourceView>> ResourcePrefabEntries;
        public List<PrefabEntry<WorldItemView>> WorldItemsPrefabEntries;
        public List<PrefabEntry<BagItemView>> BagItemViewPrefabEntries;
        public Localization Localization;
        public float InteractionDistance;
        public float SpeechBubbleTime;
        public float FollowToleranceSq;
        public float FollowSpeed;
        public float RotationSpeed = 1f;
    }

    [Serializable]
    public class Localization
    {
        [SerializeField] private List<ObjectTitleEntry> _objectTitleEntries = new();

        [SerializeField] private List<CommonModelEntry> _commonModelEntries = new();
        [SerializeField] private List<JobEntry> _jobEntries = new();

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

        public string GetCommonModelTypeTitle(string id)
        {
            return _commonModelEntries.FirstOrDefault(x => x.Id == id)?.Title;
        }
        public string GetJobTitle(JobEnum jobEnum)
        {
            return _jobEntries.FirstOrDefault(x => x.Job == jobEnum)?.Title;
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
            public JobEnum Job;
            public string Title;
        }

    }

    [Serializable]
    public class PrefabEntry<T>
    {
        public string Id;
        public T Prefab;
    }
}