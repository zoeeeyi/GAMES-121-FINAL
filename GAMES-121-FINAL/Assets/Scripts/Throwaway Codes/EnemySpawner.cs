using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyCounterAgent m_enemyCounterAgent;
    [SerializeField] GameObject[] m_enemyPrefabs;
    [SerializeField] Transform[] m_spawnPositions;
    [SerializeField] int totalEnemyCount = 3;
    Dictionary<Vector2, GameObject> m_inventory = new Dictionary<Vector2, GameObject>();

    private void Start()
    {
        for (int i = 0; i < m_spawnPositions.Length; i++)
        {
            m_inventory.Add(m_spawnPositions[i].position, null);
        }

        m_enemyCounterAgent.UpdateEnemyCount.AddListener(CheckCondition);
    }

    void CheckCondition(int _count)
    {
        if (_count < totalEnemyCount) SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        Vector2 _spawnPosition;
        int _attempt = 0;
        while (true)
        {
            _attempt++;
            if (_attempt > 5)
            {
                StopCoroutine(DelayedSpawnEnemy());
                StartCoroutine(DelayedSpawnEnemy());
                return;
            }
            _spawnPosition = m_spawnPositions[Random.Range(0, m_spawnPositions.Length)].position;
            if (m_inventory[_spawnPosition] == null) break;
        }
        GameObject _new = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)], 
            _spawnPosition, Quaternion.identity, transform);
        m_inventory[_spawnPosition] = _new;
        EnemyCounter.instance?.UpdateEnemyCount();
    }

    private void OnDestroy()
    {
        m_enemyCounterAgent.UpdateEnemyCount.RemoveListener(CheckCondition);
    }

    IEnumerator DelayedSpawnEnemy()
    {
        yield return new WaitForSeconds(2);
        SpawnEnemy();
    }
}
