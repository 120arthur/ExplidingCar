using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class Spawner : MonoBehaviour
{
    [Inject]
    private ISpawnController m_spawnController;

    [SerializeReference]
    private float m_delayAutoSpawn = 30f;
    [SerializeField]
    private int m_maxNPCs = 15;
    [SerializeField]
    private GameObject m_NPCGameObject;
    [SerializeField]
    private NPCType m_npcType;
    [SerializeField]
    private AssetReference m_npcAssetReference;

    AsyncOperationHandle<GameObject> m_npcHandle;

    private void Start()
    {
        m_npcHandle = Addressables.LoadAssetAsync<GameObject>(m_npcAssetReference);
        m_npcHandle.Completed += OnLoadComplete;
    }

    private void OnDestroy()
    {
        Addressables.Release(m_npcHandle);
    }

    private void OnLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            StartCoroutine(WaitToSpawnElvis());
        }
        else
        {
            Debug.LogError("Failed to load Addressable prefab.");
        }
    }

    private IEnumerator WaitToSpawnElvis()
    {
        while (true)
        {
            m_spawnController.InitNPCFromPool(m_npcType, transform.position, m_npcHandle.Result);
            yield return new WaitForSeconds(m_delayAutoSpawn);
        }
    }
}
