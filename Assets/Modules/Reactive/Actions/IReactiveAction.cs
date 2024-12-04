namespace Modules.Reactive.Actions
{
    public interface IReactiveAction
    {
        public void Invoke();
    }

    public interface IAction<in T>
    {
        void Invoke(T args);
    }

    public interface IAction<in T1, in T2>
    {
        void Invoke(T1 arg1, T2 arg2);
    }
}