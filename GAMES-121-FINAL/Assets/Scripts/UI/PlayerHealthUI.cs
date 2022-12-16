using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] PlayerHealthAgent so_playerHealthAgent;
    [SerializeField] Sprite[] m_healthBars;
    Image m_healthBarUI;

    private void Awake()
    {
        //Fetch components
        m_healthBarUI = GetComponent<Image>();

        //Add listener to update player health event
        so_playerHealthAgent.UpdatePlayerHealth.RemoveAllListeners();
        so_playerHealthAgent.UpdatePlayerHealth.AddListener(UpdateUI);
    }

    void UpdateUI(int _currentHealth)
    {
        if (_currentHealth < m_healthBars.Length) m_healthBarUI.sprite = m_healthBars[_currentHealth];
    }

    private void OnDestroy()
    {
        so_playerHealthAgent.UpdatePlayerHealth.RemoveListener(UpdateUI);
    }
}
