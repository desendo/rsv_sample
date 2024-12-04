using System.Threading.Tasks;

namespace Modules.Common
{
    public interface IInit
    {
        void Init();
    }
    public interface ILateInit
    {
        void LateInit();
    }
    public interface IUpdate
    {
        void Update(float dt);
    }
    public interface DataHandler<T>
    {
        void SaveTo(T data);
        void LoadFrom(T data);
    }

    public interface IGameLoad
    {
        string LoadId { get; }
        Task Load();
    }
}