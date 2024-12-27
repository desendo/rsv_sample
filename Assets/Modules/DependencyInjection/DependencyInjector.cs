using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.DependencyInjection
{
    internal class DependencyInjector
    {
        private readonly DependencyContainer container;


        internal DependencyInjector(DependencyContainer container)
        {
            this.container = container;
        }

        internal object[] GetArguments(ParameterInfo[] parameters)
        {
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var type = parameter.ParameterType;
                object arg = null;
                if (type.IsGenericType
                    && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    arg = this.container.Get(type);
                    if (arg == null)
                    {
                        var itemType = type.GetGenericArguments().Single();
                        var cachedObjectsList = this.container.All(itemType) as List<object>;
                        var genericListType = typeof(List<>).MakeGenericType(new Type[] { itemType });
                        var genericListInstance = (IList)Activator.CreateInstance(genericListType);

                        if (cachedObjectsList != null)
                        {
                            foreach (var o in cachedObjectsList)
                            {
                                genericListInstance.Add(o);
                            }
                        }

                        arg = genericListInstance;
                    }
                }
                else
                {
                    arg = this.container.Get(type);
                }

                if (arg == null)
                {
                    Console.WriteLine($"cant resolve by type {type}");
                }

                args[i] = arg;
            }

            return args;
        }

        internal void Inject(object target)
        {
            var type = target.GetType();
            var methods = type.GetMethods(BindingFlags.Instance
                                          | BindingFlags.Public
                                          | BindingFlags.NonPublic
                                          | BindingFlags.FlattenHierarchy);

            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(InjectAttribute)))
                {
                    this.InvokeMethod(method, target);
                }
            }


        }

        private void InvokeMethod(MethodInfo method, object target)
        {
            var parameters = method.GetParameters();
            var args = this.GetArguments(parameters);
            method.Invoke(target, args);
        }
    }
}