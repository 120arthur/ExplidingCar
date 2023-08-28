using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

/// <summary>
/// Enumerates the different types of NPCs.
/// </summary>
public enum NPCType
{
    NPCBlack,
    NPCRed,
    NPCWhite,
    NPCsBlue
}

/// <summary>
/// Controls the behavior of NPCs in the game.
/// </summary>
public class NPCBehaviorController : MonoBehaviour
{
    private const float INIT_NPC_CLONE_COOLDOWN = 3;

    [Inject(Id = "NPCSpawner")]
    private ISpawner<NPCType> m_spawnerNpc;

    [Inject(Id = "ParticleSpawner")]
    private ISpawner<ParticleType> m_spawnParticle;

    [SerializeField]
    private NavMeshAgent m_navMeshAgent;

    [SerializeField]
    private float m_arrivalThreshold = 1;

    private bool m_isActive;
    private NPCType m_npcType;
    private WaitForSeconds m_waitTimeToClone;
    private bool m_initCloneRequested;

    public NPCType InstanceNPCType => m_npcType;
    public bool IsActive => m_isActive;

    private void FixedUpdate()
    {
        if (m_navMeshAgent.remainingDistance <= m_arrivalThreshold)
        {
            ChangeDestination();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCBehaviorController otherNPC = other.gameObject.GetComponent<NPCBehaviorController>();

        if (otherNPC != null)
        {
            HandleCollisionWithNPC(otherNPC);
        }
    }

    /// <summary>
    /// Initializes the NPC with the specified type and position.
    /// </summary>
    /// <param name="nPCType">The type of NPC.</param>
    /// <param name="position">The initial position.</param>
    public void InitNPC(NPCType nPCType, Vector3 position)
    {
        m_waitTimeToClone = new WaitForSeconds(INIT_NPC_CLONE_COOLDOWN);
        m_npcType = nPCType;
        EnableNPC(position);
    }

    private void HandleCollisionWithNPC(NPCBehaviorController otherNPC)
    {
        if (otherNPC.InstanceNPCType == m_npcType)
        {
            StartCoroutine(InitCloneCooldown());
            if (!otherNPC.m_initCloneRequested)
            {
                m_spawnerNpc.InitObjectFromPool(m_npcType, transform.position);
            }

            return;
        }

        if (otherNPC.IsActive == false)
        {
            m_spawnParticle.InitObjectFromPool(ParticleType.Explosion, transform.position);
        }

        DisableNPC();
    }

    private void DisableNPC()
    {
        m_isActive = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the NPC at the specified position.
    /// </summary>
    /// <param name="position">The position to enable the NPC at.</param>
    public void EnableNPC(Vector3 position)
    {
        gameObject.SetActive(true);
        StartCoroutine(InitCloneCooldown());
        transform.position = position;
        m_isActive = true;
        ChangeDestination();
    }

    /// <summary>
    /// Starts a timer to become available to instantiate a clone.
    /// </summary>
    /// <returns>An IEnumerator used for the coroutine.</returns>
    private IEnumerator InitCloneCooldown()
    {
        m_initCloneRequested = true;
        yield return m_waitTimeToClone;
        m_initCloneRequested = false;
    }

    /// <summary>
    /// Changes the destination of the NPC on the NavMesh.
    /// </summary>
    private void ChangeDestination()
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