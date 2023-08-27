using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class SpawnParticleController : BaseSpawner<ParticleType, ParticleHandler>
{
    [Inject]
    private IInstantiator m_instantiator;

    protected override ParticleHandler GetInactiveObject(ParticleType particleType)
    {
        List<ParticleHandler> npcList;
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

    public override void EnableObject(Vector3 position, ParticleHandler currentObject)
    {
        currentObject.StartParticle(position);
    }

    public override void CreateObject(ParticleType nPCType, Vector3 position)
    {
        bool hasReference = m_objectsOperationHandle.TryGetValue(nPCType, out var handle) && handle.Status == AsyncOperationStatus.Succeeded;

        if (m_objects.TryGetValue(nPCType, out var nPCList) && hasReference)
        {
            ParticleHandler npc = m_instantiator.InstantiatePrefabForComponent<ParticleHandler>(handle.Result);
            npc.StartParticle(position);
            nPCList.Add(npc);
        }
    }
}