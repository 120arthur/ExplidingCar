using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SpawnerPoint : MonoBehaviour
{
    public const float DEFAULT_DELAY_TO_SPAWN = 5;

    [Inject]
    private SignalBus m_signalBus;
    [Inject]
    private SpawnerNPCController m_spawnerNPCController;

    [SerializeField]
    private NPCType m_npcType;

    private WaitForSeconds m_waitForSeconds;

    private void Start()
    {
        StartCoroutine(WaitToSpawnNPC());
        m_waitForSeconds = new WaitForSeconds(DEFAULT_DELAY_TO_SPAWN);
        m_signalBus.Subscribe<OnSpawnerRateTimeChangeSignal>(ChangeSpawnRange);
    }

    private void OnDestroy()
    {
        m_signalBus.Unsubscribe<OnSpawnerRateTimeChangeSignal>(ChangeSpawnRange);
    }

    private IEnumerator WaitToSpawnNPC()
    {
        while (true)
        {
            m_spawnerNPCController.InitObjectFromPool(m_npcType, transform.position);
            yield return m_waitForSeconds;
        }
    }

    public void ChangeSpawnRange(OnSpawnerRateTimeChangeSignal args)
    {
        m_waitForSeconds = new WaitForSeconds(args.RateTime);
    }
}
