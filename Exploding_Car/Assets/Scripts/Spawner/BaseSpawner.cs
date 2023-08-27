using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

/// <summary>
/// An abstract generic class that serves as a foundation for implementing spawner functionality with generic behavior.
/// </summary>
/// <typeparam name="T">The type representing the object types to be spawned.</typeparam>
/// <typeparam name="T2">The type representing the spawned objects.</typeparam>
public abstract class BaseSpawner<T, T2> : ISpawner<T>
{
    protected Dictionary<T, AsyncOperationHandle<GameObject>> m_objectsOperationHandle = new Dictionary<T, AsyncOperationHandle<GameObject>>();
    protected Dictionary<T, List<T2>> m_objects = new Dictionary<T, List<T2>>();

    [Inject]
    private void InitOperationHandlers()
    {
        List<T> objectsList = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        foreach (T objectType in objectsList)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(objectType.ToString());
            m_objectsOperationHandle.Add(objectType, handle);
        }
    }

    public void InitObjectFromPool(T objectType, Vector3 position)
    {
        if (!m_objects.ContainsKey(objectType))
        {
            m_objects.Add(objectType, new List<T2>());
        }

        T2 Object = GetInactiveObject(objectType);

        if (Object == null)
        {
            InstantiateObject(objectType, position);
        }
        else
        {
            EnableObject(position, Object);
        }
    }

    /// <summary>
    /// Method to get an inactive object of a specific type.
    /// </summary>
    /// <param name="objectType">The type of object to retrieve.</param>
    /// <returns>An inactive object of the specified type.</returns>
    protected abstract T2 GetInactiveObject(T objectType);

    /// <summary>
    /// Method to instantiate an object of a specific type.
    /// </summary>
    /// <param name="objectType">The type of object to instantiate.</param>
    /// <param name="position">The position to place the object.</param>
    protected abstract void InstantiateObject(T objectType, Vector3 position);

    /// <summary>
    /// Method to enable an object at a specific position.
    /// </summary>
    /// <param name="position">The position to enable the object at.</param>
    /// <param name="currentObject">The object to enable.</param>
    protected abstract void EnableObject(Vector3 position, T2 currentObject);
}