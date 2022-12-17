using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCountUI : MonoBehaviour
{
    [SerializeField] EnemyCounterAgent m_enemyCounterAgent;
    TextMeshProUGUI m_enemyCountText;

    private void Awake()
    {
        m_enemyCountText = GetComponent<TextMeshProUGUI>();
        m_enemyCounterAgent.UpdateEnemyCount.AddListener(UpdateEnemyCountUI);
    }

    void UpdateEnemyCountUI(int _count)
    {
        m_enemyCountText.text = _count.ToString();
    }

    private void OnDestroy()
    {
        m_enemyCounterAgent.UpdateEnemyCount.RemoveListener(UpdateEnemyCountUI);
    }
}
