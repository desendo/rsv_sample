using System.Collections;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.DependencyInjection
{
    public class DependencyContainer
    {
        private readonly Cache cache;

        private readonly DependencyInjector injector;

        public DependencyContainer()
        {
            this.cache = new Cache();
            this.injector = new DependencyInjector(this);
        }

        /// <summary>
        /// Creates new and return instance, whithout injecting, adds to container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Add<T>()
            where T : class
        {
            var constructors = typeof(T).GetConstructors();

            object instance;
            if (constructors.Length != 1
                || !this.HasDefaultConstructor(typeof(T)))
            {
                instance = (T)FormatterServices.GetUninitializedObject(typeof(T));
            }
            else
            {
                instance = Activator.CreateInstance(typeof(T));
            }

            this.cache.Add(instance);
            return (T)instance;
        }

        public void Add(object target)
        {
            this.cache.Add(target);
        }
        public void Add<T>(object target)
        {
            this.cache.Add<T>(target);
        }
        public void AddInject(object target)
        {
            this.cache.Add(target);
            this.injector.Inject(target);
        }

        /// <summary>
        /// Add and return instance, injecting its deps, adds to container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T AddInject<T>()
            where T : class
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();
            object instance;

            if (constructors.Length > 1)
            {
                throw new Exception("trying to bind multiple constructor object. can't choose.");
            }

            if (constructors.Length == 1
                || this.HasDefaultConstructor(typeof(T)))
            {
                var paramInfos = constructors[0].GetParameters();
                var args = this.injector.GetArguments(paramInfos);
                instance = Activator.CreateInstance(typeof(T), args);
            }
            else
            {
                instance = (T)FormatterServices.GetUninitializedObject(typeof(T));
            }

            this.cache.Add(instance);
            this.injector.Inject(instance);
            return (T)instance;
        }
        /// <summary>
        /// Creates and return instance, injecting its deps, does not add to container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T CreateInject<T>()
            where T : class
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();
            object instance;

            if (constructors.Length > 1)
            {
                throw new Exception("trying to bind multiple constructor object. can't choose.");
            }

            if (constructors.Length == 1
                || this.HasDefaultConstructor(typeof(T)))
            {
                var paramInfos = constructors[0].GetParameters();
                var args = this.injector.GetArguments(paramInfos);
                instance = Activator.CreateInstance(typeof(T), args);
            }
            else
            {
                instance = (T)FormatterServices.GetUninitializedObject(typeof(T));
            }

            return (T)instance;
        }

        public object Get(Type type)
        {
            return this.cache.Get(type);
        }

        public T Get<T>()
        {
            return (T)this.cache.Get(typeof(T));
        }

        internal object All(Type type)
        {
            return this.cache.All(type);
        }

        public List<T> All<T>()
        {
            if (this.cache.All(typeof(T)) == null)
            {
                return null;
            }

            var cached = ((List<T>)this.cache.All(typeof(T)));
            var list = cached.ToList();
            return list;
        }

        public void Inject(object target)
        {
            this.injector.Inject(target);
        }

        private bool HasDefaultConstructor(Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }

        private class Cache
        {
            private readonly Dictionary<Type, List<object>> objectsByInterfaces = new();

            private readonly Dictionary<Type, object> objectsByTypes = new();

            public void Add<T>(object target)
            {
                if (target == null)
                    throw new NullReferenceException("adding null target to di cache");

                var type = typeof(T);
                var interfaces = type.GetInterfaces();
                if (!this.objectsByTypes.ContainsKey(type))
                {
                    this.objectsByTypes[type] = target;
                }
                else
                {
                    Console.WriteLine($"{type} is already bound to {target.GetType()} ");
                }

                foreach (var i in interfaces)
                {
                    if (!this.objectsByInterfaces.ContainsKey(i))
                    {
                        this.objectsByInterfaces.Add(i, new List<object>());
                    }

                    if (this.objectsByInterfaces[i].Contains(target))
                    {
                        Console.WriteLine($"{target.GetType()} already add to {i} list");
                        continue;
                    }
                    else
                    {
                        this.objectsByInterfaces[i].Add(target);
                    }
                }
            }

            public void Add(object target)
            {
                var type = target.GetType();
                var interfaces = type.GetInterfaces();
                if (!this.objectsByTypes.ContainsKey(type))
                {
                    this.objectsByTypes[type] = target;
                }
                else
                {
                    Console.WriteLine($"{type} is already bound to {target.GetType()} ");
                }

                foreach (var i in interfaces)
                {
                    if (!this.objectsByInterfaces.ContainsKey(i))
                    {
                        this.objectsByInterfaces.Add(i, new List<object>());
                    }

                    if (this.objectsByInterfaces[i].Contains(target))
                    {
                        Console.WriteLine($"{target.GetType()} already add to {i} list");
                        continue;
                    }
                    else
                    {
                        this.objectsByInterfaces[i].Add(target);
                    }
                }
            }

            public object Get(Type type)
            {
                if (this.objectsByTypes.TryGetValue(type, out var obj))
                {
                    return obj;
                }

                if (this.objectsByInterfaces.TryGetValue(type, out var list))
                {
                    if (list.Count == 1)
                    {
                        return list[0];
                    }

                    throw new Exception($"trying to resolve 1 element of type {type} from list count {list.Count}");
                }

                return null;
            }

            internal object All(Type type)
            {
                if (this.objectsByInterfaces.TryGetValue(type, out var list))
                {
                    return list;
                }

                var genericListType = typeof(List<>).MakeGenericType(new Type[] { type });
                var genericListInstance = (IList)Activator.CreateInstance(genericListType);
                return genericListInstance;
            }
        }
    }
}