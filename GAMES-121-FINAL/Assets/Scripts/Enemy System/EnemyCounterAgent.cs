using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Enemy Counter Agent", menuName = "Scriptable Objects/Enemy Counter Agent")]
public class EnemyCounterAgent : ScriptableObject
{
    [HideInInspector] public UnityEvent<int> UpdateEnemyCount;
    public int enemyCount { get; private set; }

    public void SetEnemyCount(int _count)
    {
        enemyCount = _count;
        UpdateEnemyCount.Invoke(_count);
    }

    public void ChangeEnemyCount(int _changeAmount)
    {
        enemyCount += _changeAmount;
        UpdateEnemyCount.Invoke(enemyCount);
    }
}
