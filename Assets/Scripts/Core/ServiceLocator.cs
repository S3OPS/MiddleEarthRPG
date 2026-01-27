using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Service Locator pattern implementation for loose coupling between systems.
    /// Implements the "Break up the Fellowship" principle - modularization.
    /// 
    /// Instead of direct singleton references, systems can register themselves
    /// and be retrieved through this centralized service registry.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static bool _isInitialized = false;
        
        /// <summary>
        /// Register a service implementation.
        /// </summary>
        /// <typeparam name="T">The service type (usually an interface).</typeparam>
        /// <param name="service">The service implementation.</param>
        public static void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                Debug.LogWarning($"ServiceLocator: Attempted to register null service for type {typeof(T).Name}");
                return;
            }
            
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: Overwriting existing service for type {type.Name}");
            }
            
            _services[type] = service;
            _isInitialized = true;
        }
        
        /// <summary>
        /// Get a registered service.
        /// </summary>
        /// <typeparam name="T">The service type to retrieve.</typeparam>
        /// <returns>The service implementation, or null if not found.</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return service as T;
            }
            
            return null;
        }
        
        /// <summary>
        /// Try to get a registered service.
        /// </summary>
        /// <typeparam name="T">The service type to retrieve.</typeparam>
        /// <param name="service">The service implementation if found.</param>
        /// <returns>True if the service was found.</returns>
        public static bool TryGet<T>(out T service) where T : class
        {
            service = Get<T>();
            return service != null;
        }
        
        /// <summary>
        /// Check if a service is registered.
        /// </summary>
        /// <typeparam name="T">The service type to check.</typeparam>
        /// <returns>True if the service is registered.</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }
        
        /// <summary>
        /// Unregister a service.
        /// </summary>
        /// <typeparam name="T">The service type to unregister.</typeparam>
        public static void Unregister<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
        }
        
        /// <summary>
        /// Clear all registered services. Typically called on scene unload or game reset.
        /// </summary>
        public static void Clear()
        {
            _services.Clear();
            _isInitialized = false;
        }
        
        /// <summary>
        /// Whether any services have been registered.
        /// </summary>
        public static bool IsInitialized => _isInitialized;
        
        /// <summary>
        /// Get the count of registered services.
        /// </summary>
        public static int ServiceCount => _services.Count;
    }
}
