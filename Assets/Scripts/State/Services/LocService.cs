using System.Collections.Generic;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public static class LocService
    {
        public static IReactiveVariable<SystemLanguage> CurrentLanguage { get; } = new ReactiveVariable<SystemLanguage>(SystemLanguage.English);
        private static readonly Dictionary<string, IReactiveVariable<string>> _strings = new Dictionary<string, IReactiveVariable<string>>();
        public static void Init(Localization.Language language)
        {
            foreach (var entry in language.Strings)
            {
                if (_strings.TryGetValue(entry.Id, out var s))
                    s.Value = entry.Value;
                else
                {
                    _strings.Add(entry.Id, new ReactiveVariable<string>(entry.Value));
                }
            }

            foreach (var (key, value) in _strings)
            {
                int count = 0;
                foreach (var x in language.Strings)
                    if (x.Id == key)
                        count++;

                if (count == 0) value.Value = string.Concat(CurrentLanguage.Value.ToString(), "_", key);
            }
        }

        public static IReactiveVariable<string> ById(string id)
        {
            if (!_strings.TryGetValue(id, out var reactiveVariable))
            {
                reactiveVariable = new ReactiveVariable<string>(string.Concat(CurrentLanguage.Value.ToString(),"_",id));
                _strings.Add(id, reactiveVariable);
            }
            return reactiveVariable;
        }
    }
}