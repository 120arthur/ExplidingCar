using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// A concrete implementation of a spawner controller for NPCs.
/// Inherits from the generic BaseSpawner class.
/// </summary>
public class NPCSpawnerController : BaseSpawner<NPCType, NPCBehaviorController>
{
    private const int MAX_NPCS_OF_TYPE = 200;

    [Inject]
    private IInstantiator m_instantiator;

    protected override NPCBehaviorController GetInactiveObject(NPCType nPCType)
    {
        List<NPCBehaviorController> npcList;
        if (m_objects.TryGetValue(nPCType, out npcList))
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

    protected override void EnableObject(Vector3 position, NPCBehaviorController currentObject)
    {
        currentObject.EnableNPC(position);
    }

    protected override void InstantiateObject(NPCType npcType, Vector3 position)
    {
        bool hasReference = m_objectsOperationHandle.TryGetValue(npcType, out var handle) && handle.Status == AsyncOperationStatus.Succeeded;

        if (m_objects.TryGetValue(npcType, out var npcList) && npcList.Count < MAX_NPCS_OF_TYPE && hasReference)
        {
            NPCBehaviorController npc = m_instantiator.InstantiatePrefabForComponent<NPCBehaviorController>(handle.Result);
            npc.InitNPC(npcType, position);
            npcList.Add(npc);
        }
    }

}