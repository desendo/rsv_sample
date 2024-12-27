using System.Collections;
using System.Collections.Generic;

namespace Game.State.Models
{
    public interface ISaveData<in T>
    {
        void SaveTo(T data);
    }
    public interface ISaveLoadData<T> : ILoadData<T>, ISaveData<T>
    {
    }
    public interface ILoadData<T>
    {
        void LoadFrom(in T data);
    }

    public interface IModelEnum<T>
    {
        IEnumerable<T> GetEnum();
    }

    public interface IReset
    {
        void Reset();
    }
}