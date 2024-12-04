namespace Modules.DependencyInjection
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }
}