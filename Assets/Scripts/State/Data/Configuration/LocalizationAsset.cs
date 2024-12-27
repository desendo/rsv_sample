using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Create LocalizationAsset", fileName = "LocalizationAsset", order = 0)]
    public class LocalizationAsset :ScriptableObject
    {

        [SerializeField] public List<Language> Languages = new();

        [Serializable]
        public class IdString
        {
            public string Id;
            public string Value;
        }
        [Serializable]
        public class Language
        {
            public SystemLanguage Id;
            public List<IdString> Strings;
        }
    }
}