using UnityEngine;

namespace Game.State.Config
{
    public abstract class DataContainerAbstract<T> : ScriptableObject
    {
        public T Data;
    }
}