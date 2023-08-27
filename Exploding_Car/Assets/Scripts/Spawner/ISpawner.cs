using UnityEngine;

public interface ISpawner<T>
{
    void InitObjectFromPool(T objectType, Vector3 position);
}