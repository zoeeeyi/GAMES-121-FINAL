using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelOverlay : MonoBehaviour
{
    [SerializeField] GameData m_gameData;
    private void Awake()
    {
        m_gameData.GAME_ContinueLevel.AddListener(DisableSelf);
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_gameData.GAME_ContinueLevel.RemoveListener(DisableSelf);
    }
}
