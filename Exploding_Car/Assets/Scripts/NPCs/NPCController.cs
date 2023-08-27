using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public enum NPCType
{
    NPCBlack,
    NPCRed,
    NPCWhite,
    NPCsBlue
}

public class NPCController : MonoBehaviour
{
    private const float INIT_NPC_CLONE_COOLDOWN = 3;
    private const int NPC_LAYER_ID = 3;
    [Inject]
    private SpawnerNPCController m_spawnerNPCController;
    [Inject]
    private SpawnParticleController m_spawnParticleController;

    [SerializeField]
    private NavMeshAgent m_navMeshAgent;

    [SerializeField]
    private float arrivalThreshold = 1;

    private bool m_isActive;

    private NPCType m_npcType;

    private WaitForSeconds m_waitTimeToClone;
    private bool m_initCloneRequested;

    public NPCType InstanceNPCType => m_npcType;
    public bool IsActive => m_isActive;

    private void FixedUpdate()
    {
        if (m_navMeshAgent.remainingDistance <= arrivalThreshold)
        {
            ChangeDestination();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCController otherNPC = other.gameObject.GetComponent<NPCController>();

        if (otherNPC != null)
        {
            HandleCollisionWithNPC(otherNPC);
        }
    }

    public void Init(NPCType nPCType, Vector3 position)
    {
        m_waitTimeToClone = new WaitForSeconds(INIT_NPC_CLONE_COOLDOWN);
        m_npcType = nPCType;
        EnableNPC(position);
    }

    private void HandleCollisionWithNPC(NPCController otherNPC)
    {
        if (otherNPC.InstanceNPCType == m_npcType)
        {
            StartCoroutine(InitCloneCooldown());
            if (!otherNPC.m_initCloneRequested)
            {
                m_spawnerNPCController.InitObjectFromPool(m_npcType, transform.position);
            }

            return;
        }

        if (otherNPC.IsActive == false)
        {
            m_spawnParticleController.InitObjectFromPool(ParticleType.Explosion, transform.position);
        }

        DisableNPC();
    }

    private void DisableNPC()
    {
        m_isActive = false;
        gameObject.SetActive(false);
    }

    public void EnableNPC(Vector3 position)
    {
        gameObject.SetActive(true);
        StartCoroutine(InitCloneCooldown());
        transform.position = position;
        m_isActive = true;
        ChangeDestination();
    }

    private IEnumerator InitCloneCooldown()
    {
        m_initCloneRequested = true;
        yield return m_waitTimeToClone;
        m_initCloneRequested = false;
    }

    public void ChangeDestination()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int randomTriangleIndex = Random.Range(0, navMeshData.indices.Length / 3);

        Vector3 v1 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3]];
        Vector3 v2 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 1]];
        Vector3 v3 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 2]];

        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(0f, 1f);

        if (randomX + randomY > 1)
        {
            randomX = 1 - randomX;
            randomY = 1 - randomY;
        }

        Vector3 randomPointInTriangle = v1 + randomX * (v2 - v1) + randomY * (v3 - v1);

        m_navMeshAgent.SetDestination(randomPointInTriangle);
    }

}