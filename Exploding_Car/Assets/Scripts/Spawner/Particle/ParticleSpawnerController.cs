using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
/// <summary>
/// A concrete implementation of a spawner controller for Particles.
/// Inherits from the generic BaseSpawner class.
/// </summary>
public class ParticleSpawnerController : BaseSpawner<ParticleType, ParticleEffectHandler>
{
    [Inject]
    private IInstantiator m_instantiator;

    protected override ParticleEffectHandler GetInactiveObject(ParticleType particleType)
    {
        if (m_objects.TryGetValue(particleType, out List<ParticleEffectHandler> particleList))
        {
            foreach (var particle in particleList)
            {
                if (!particle.IsActive)
                {
                    return particle;
                }
            }
        }
        return null;
    }

    protected override void EnableObject(Vector3 position, ParticleEffectHandler currentObject)
    {
        currentObject.StartParticle(position);
    }

    protected override void InstantiateObject(ParticleType particleType, Vector3 position)
    {
        bool hasReference = m_objectsOperationHandle.TryGetValue(particleType, out var handle) && handle.Status == AsyncOperationStatus.Succeeded;

        if (m_objects.TryGetValue(particleType, out var particleList) && hasReference)
        {
            ParticleEffectHandler particle = m_instantiator.InstantiatePrefabForComponent<ParticleEffectHandler>(handle.Result);
            particle.InitPartice(position);
            particleList.Add(particle);
        }
        else
        {
            Debug.LogError($"Failed to load particle reference for {particleType}.");
        }
    }
}