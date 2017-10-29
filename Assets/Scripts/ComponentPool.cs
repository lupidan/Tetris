using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class ComponentPool<T> where T : MonoBehaviour
    {
        private Func<T> _instantiateAction;
        private Action<T> _getComponentAction;
        private Action<T> _returnComponentAction;
        private Stack<T> _pooledObjects;
        private int _lastInstantiatedAmount;

        public ComponentPool(int initialPoolSize, Func<T> instantiateFunction, Action<T> getComponentAction = null, Action<T> returnComponentAction = null)
        {
            this._instantiateAction = instantiateFunction;
            this._getComponentAction = getComponentAction;
            this._returnComponentAction = returnComponentAction;
            this._pooledObjects = new Stack<T>();
            InstantiateComponentsIntoPool(initialPoolSize);
        }

        public T Get()
        {
            if (_pooledObjects.Count == 0)
                InstantiateComponentsIntoPool((_lastInstantiatedAmount * 2) + 1);

            T component = _pooledObjects.Pop();
            if (_getComponentAction != null)
                _getComponentAction(component);

            return component;
        }

        public void Return(T component)
        {
            _pooledObjects.Push(component);
            if (_returnComponentAction != null)
                _returnComponentAction(component);
        }

        private void InstantiateComponentsIntoPool(int nComponents)
        {
            for (int i = 0; i < nComponents; i++)
            {
                var pooledObject = _instantiateAction();
                _pooledObjects.Push(pooledObject);
            }
            
            _lastInstantiatedAmount = _pooledObjects.Count;
        }
    }
}
