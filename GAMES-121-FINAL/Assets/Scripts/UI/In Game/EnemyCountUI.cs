using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCountUI : MonoBehaviour
{
    [SerializeField] EnemyCounterAgent m_enemyCounterAgent;
    [SerializeField] TextMeshProUGUI m_enemyCountText;

    private void Awake()
    {
        m_enemyCounterAgent.UpdateEnemyCount.AddListener(UpdateEnemyCountUI);
    }

    void UpdateEnemyCountUI(int _count)
    {
        m_enemyCountText.text = _count.ToString();
    }

    //For animation
    public void ShakeCamera(float _intensity)
    {
        CameraController.instance.CameraShake(_intensity, 0.5f);
    }

    private void OnDestroy()
    {
        m_enemyCounterAgent.UpdateEnemyCount.RemoveListener(UpdateEnemyCountUI);
    }
}
