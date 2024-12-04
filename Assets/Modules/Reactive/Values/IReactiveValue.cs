namespace Modules.Reactive.Values
{
    public interface IReactiveValue<out T>
    {
        T Value { get; }
    }
}