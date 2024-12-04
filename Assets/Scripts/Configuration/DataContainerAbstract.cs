using UnityEngine;

namespace Game.Configuration
{
    public abstract class DataContainerAbstract<T> : ScriptableObject
    {
        public T Data;
    }
}