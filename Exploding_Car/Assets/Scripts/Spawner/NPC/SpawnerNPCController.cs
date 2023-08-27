using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;


public class SpawnerNPCController : BaseSpawner<NPCType, NPCController>
{
    private const int MAX_NPCS_OF_TYPE = 200;

    [Inject]
    private IInstantiator m_instantiator;

    protected override NPCController GetInactiveObject(NPCType nPCType)
    {
        List<NPCController> npcList;
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

    public override void EnableObject(Vector3 position, NPCController currentObject)
    {
        currentObject.EnableNPC(position);
    }

    public override void CreateObject(NPCType nPCType, Vector3 position)
    {
        bool hasReference = m_objectsOperationHandle.TryGetValue(nPCType, out var handle) && handle.Status == AsyncOperationStatus.Succeeded;

        if (m_objects.TryGetValue(nPCType, out var nPCList) && nPCList.Count < MAX_NPCS_OF_TYPE && hasReference)
        {
            NPCController npc = m_instantiator.InstantiatePrefabForComponent<NPCController>(handle.Result);
            npc.Init(nPCType, position);
            nPCList.Add(npc);
        }
    }

}