using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [HideInInspector] public static EnemyCounter instance;
    [SerializeField] EnemyCounterAgent m_enemyCounterAgent;
    [SerializeField] bool m_resetEnemyCount = true;
    int m_localEnemyCount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_localEnemyCount = transform.childCount;
        if (m_resetEnemyCount) m_enemyCounterAgent.SetEnemyCount(m_localEnemyCount);
        else m_enemyCounterAgent.ChangeEnemyCount(m_localEnemyCount);
    }

    public void UpdateEnemyCount()
    {
        int _count = transform.childCount;
        int _changeAmount = _count - m_localEnemyCount;
        m_localEnemyCount = _count;
        m_enemyCounterAgent.ChangeEnemyCount(_changeAmount);
    }
}
