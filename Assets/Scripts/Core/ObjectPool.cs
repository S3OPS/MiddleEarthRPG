using UnityEngine;
using System;
using System.Collections.Generic;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Simple object pool for generic GameObjects.
    /// Implements the "Make the Journey Faster" principle - optimization.
    /// 
    /// Reuses objects instead of creating/destroying them repeatedly,
    /// reducing garbage collection pressure and improving performance.
    /// </summary>
    public class ObjectPool
    {
        private readonly Func<GameObject> _createFunc;
        private readonly Action<GameObject> _onGet;
        private readonly Action<GameObject> _onRelease;
        private readonly Queue<GameObject> _pool;
        private readonly int _maxSize;
        private readonly Transform _poolParent;
        
        /// <summary>
        /// Create a new object pool.
        /// </summary>
        /// <param name="createFunc">Function to create new objects when pool is empty.</param>
        /// <param name="onGet">Action to perform when getting an object from the pool.</param>
        /// <param name="onRelease">Action to perform when returning an object to the pool.</param>
        /// <param name="initialSize">Initial number of objects to pre-create.</param>
        /// <param name="maxSize">Maximum pool size before objects are destroyed instead of pooled.</param>
        /// <param name="poolParent">Optional parent transform for pooled objects.</param>
        public ObjectPool(
            Func<GameObject> createFunc,
            Action<GameObject> onGet = null,
            Action<GameObject> onRelease = null,
            int initialSize = 10,
            int maxSize = 100,
            Transform poolParent = null)
        {
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _onGet = onGet;
            _onRelease = onRelease;
            _maxSize = maxSize;
            _poolParent = poolParent;
            _pool = new Queue<GameObject>(initialSize);
            
            // Pre-populate the pool
            for (int i = 0; i < initialSize; i++)
            {
                var obj = CreateNewObject();
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
        
        /// <summary>
        /// Get an object from the pool, or create a new one if the pool is empty.
        /// </summary>
        /// <returns>A GameObject ready for use.</returns>
        public GameObject Get()
        {
            GameObject obj;
            
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = CreateNewObject();
            }
            
            obj.SetActive(true);
            _onGet?.Invoke(obj);
            
            return obj;
        }
        
        /// <summary>
        /// Return an object to the pool.
        /// </summary>
        /// <param name="obj">The object to return.</param>
        public void Release(GameObject obj)
        {
            if (obj == null) return;
            
            _onRelease?.Invoke(obj);
            obj.SetActive(false);
            
            if (_pool.Count < _maxSize)
            {
                if (_poolParent != null)
                {
                    obj.transform.SetParent(_poolParent);
                }
                _pool.Enqueue(obj);
            }
            else
            {
                // Pool is full, destroy the object
                UnityEngine.Object.Destroy(obj);
            }
        }
        
        /// <summary>
        /// Clear all objects from the pool.
        /// </summary>
        public void Clear()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                if (obj != null)
                {
                    UnityEngine.Object.Destroy(obj);
                }
            }
        }
        
        /// <summary>
        /// Get the current number of objects in the pool.
        /// </summary>
        public int Count => _pool.Count;
        
        /// <summary>
        /// Get the maximum pool size.
        /// </summary>
        public int MaxSize => _maxSize;
        
        private GameObject CreateNewObject()
        {
            var obj = _createFunc();
            if (_poolParent != null)
            {
                obj.transform.SetParent(_poolParent);
            }
            return obj;
        }
    }
}
