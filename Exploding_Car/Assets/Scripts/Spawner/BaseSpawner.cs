using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

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
            CreateObject(objectType, position);
        }
        else
        {
            EnableObject(position, Object);
        }
    }

    protected abstract T2 GetInactiveObject(T objectType);

    public abstract void CreateObject(T objectType, Vector3 position);
    public abstract void EnableObject(Vector3 position, T2 currentObject);
}