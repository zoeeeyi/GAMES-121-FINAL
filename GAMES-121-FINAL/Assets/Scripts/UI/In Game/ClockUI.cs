using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] GameData m_gameData;
    TextMeshProUGUI m_clockUI;
    bool m_enabled = false;

    private void Awake()
    {
        m_clockUI = GetComponent<TextMeshProUGUI>();
        m_gameData.GAME_StartLevelPrep.AddListener(EnableClock);
    }

    private void Update()
    {
        if (Clock.instance == null || !m_enabled) return;
        if(!Clock.instance.state_paused) m_clockUI.text = Clock.instance.GetCurrentTimeString();
    }

    void EnableClock()
    {
        m_enabled = true;
        m_clockUI.text = Clock.instance.GetCurrentTimeString();

        //Insert disable clock code here if there's a mode that needs this.
    }

    private void OnDestroy()
    {
        m_gameData.GAME_StartLevelPrep.RemoveListener(EnableClock);
    }
}
