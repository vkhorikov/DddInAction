using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DddInPractice.Logic.Common
{
    public static class DomainEvents_old
    {
        private static Dictionary<Type, List<Delegate>> _dynamicHandlers;
        private static List<Type> _staticHandlers;

        public static void Init()
        {
            _dynamicHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface)
                .ToList()
                .ToDictionary(x => x, x => new List<Delegate>());

            _staticHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
                .ToList();
        }

        public static void Register<T>(Action<T> eventHandler)
            where T : IDomainEvent
        {
            _dynamicHandlers[typeof(T)].Add(eventHandler);
        }

        public static void Raise<T>(T domainEvent)
            where T : IDomainEvent
        {
            foreach (Delegate handler in _dynamicHandlers[domainEvent.GetType()])
            {
                var action = (Action<T>)handler;
                action(domainEvent);
            }

            foreach (Type handler in _staticHandlers)
            {
                if (typeof(IHandler<T>).IsAssignableFrom(handler))
                {
                    IHandler<T> instance = (IHandler<T>)Activator.CreateInstance(handler);
                    instance.Handle(domainEvent);
                }
            }
        }
    }
}
