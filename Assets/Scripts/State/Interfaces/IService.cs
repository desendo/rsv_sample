using System.Collections.Generic;

namespace Game.State.Models
{
    public interface ISaveData<in T>
    {
        void SaveTo(T data);
    }

    public interface ILoadData<T>
    {
        void LoadFrom(in T data);
    }

    public interface IModelList<T>
    {
        List<T> GetList();
    }

    public interface IReset
    {
        void Reset();
    }
}