using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnController : ISpawnController
{
    [Inject]
    IInstantiator m_Instantiator;

    Dictionary<NPCType, List<NPC>> m_NPCs = new Dictionary<NPCType, List<NPC>>();

    public void InitNPCFromPool(NPCType nPCType, Vector3 position, GameObject npc)
    {
        if (m_NPCs[nPCType] == null)
        {
            m_NPCs[nPCType] = new List<NPC>();
        }

        var nPCs = m_NPCs[nPCType];

        for (int i = 0; i < nPCs.Count; i++)
        {
            NPC nPC = nPCs[i];

            if (nPC != null && !nPC.IsActive)
            {
                nPC.EnableNPC(position);
                return;
            }
        }

        CreateAndEnableNPC(nPCType, position, npc);
    }

    private void CreateAndEnableNPC(NPCType nPCType, Vector3 position, GameObject npcObject)
    {
        NPC npc = m_Instantiator.InstantiatePrefabForComponent<NPC>(npcObject);
        npc.Init(nPCType, position);
        m_NPCs[nPCType].Add(npc);
    }
}
