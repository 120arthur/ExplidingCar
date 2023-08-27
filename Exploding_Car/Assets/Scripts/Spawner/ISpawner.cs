using UnityEngine;

public interface ISpawner<T>
{   
    /// <summary>
     /// Initializes an object from the object pool, either by activating an inactive object or instantiating a new one.
     /// </summary>
     /// <param name="objectType">The type of object to initialize.</param>
     /// <param name="position">The position to place the object.</param>
    void InitObjectFromPool(T objectType, Vector3 position);
}