using System;
using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// Represents a prefab in the game world that spawns NPCs based on a timed interval.
/// </summary>
public class DelayedSpawnPoint : MonoBehaviour
{
    public const float DEFAULT_DELAY_TO_SPAWN = 5;

    [Inject]
    private SignalBus m_signalBus;

    [Inject(Id = "NPCSpawner")]
    private ISpawner<NPCType> m_spawnerNpc;

    [SerializeField]
    private NPCType m_npcType;

    private WaitForSeconds m_waitForSeconds;

    private void Start()
    {
        StartCoroutine(WaitToSpawnNpc());
        m_waitForSeconds = new WaitForSeconds(DEFAULT_DELAY_TO_SPAWN);
        m_signalBus.Subscribe<OnSpawnerRateTimeChangeSignal>(ChangeSpawnDelay);
    }

    private void OnDestroy()
    {
        m_signalBus.Unsubscribe<OnSpawnerRateTimeChangeSignal>(ChangeSpawnDelay);
    }

    /// <summary>
    /// Coroutine to continuously spawn NPCs with a delay between each spawn.
    /// </summary>
    private IEnumerator WaitToSpawnNpc()
    {
        while (true)
        {
            m_spawnerNpc.InitObjectFromPool(m_npcType, transform.position);
            yield return m_waitForSeconds;
        }
    }

    /// <summary>
    /// Callback to change the delay between NPC spawns based on a signal.
    /// </summary>
    /// <param name="args">The signal argument containing the new delay.</param>
    public void ChangeSpawnDelay(OnSpawnerRateTimeChangeSignal args)
    {
        m_waitForSeconds = new WaitForSeconds(args.RateTime);
    }
}