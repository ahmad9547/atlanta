using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ServiceLocator
{
    public sealed class Service
    {
        public static Service Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Service();
                }

                return _instance;
            }
        }

        private static Service _instance;

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();


        public T Get<T>() where T : IService
        {
            Type key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"{key} not registered with {GetType().Name}");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }

        public void Register<T>(T service) where T : IService
        {
            Type key = typeof(T);
            if (_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to register service of type {key} which already registered with {GetType().Name}");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IService
        {
            Type key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to unregister service of type {key} which was not registered");
            }

            _services.Remove(key);
        }
    }
}