namespace Modules.Reactive.Values
{
    public interface IReactiveVariable<T> :  IReadOnlyReactiveVariable<T>
    {
        new T Value { get; set; }
        void SetForceNotify(T Value);
    }
}