using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
/// <summary>
/// A concrete implementation of a spawner controller for Particles.
/// Inherits from the generic BaseSpawner class.
/// </summary>
public class SpawnParticleController : BaseSpawner<ParticleType, ParticleController>
{
    [Inject]
    private IInstantiator m_instantiator;

    protected override ParticleController GetInactiveObject(ParticleType particleType)
    {
        List<ParticleController> npcList;
        if (m_objects.TryGetValue(particleType, out npcList))
        {
            foreach (var npc in npcList)
            {
                if (!npc.IsActive)
                {
                    return npc;
                }
            }
        }
        return null;
    }

    protected override void EnableObject(Vector3 position, ParticleController currentObject)
    {
        currentObject.StartParticle(position);
    }

    protected override void InstantiateObject(ParticleType nPCType, Vector3 position)
    {
        bool hasReference = m_objectsOperationHandle.TryGetValue(nPCType, out var handle) && handle.Status == AsyncOperationStatus.Succeeded;

        if (m_objects.TryGetValue(nPCType, out var nPCList) && hasReference)
        {
            ParticleController npc = m_instantiator.InstantiatePrefabForComponent<ParticleController>(handle.Result);
            npc.StartParticle(position);
            nPCList.Add(npc);
        }
    }
}