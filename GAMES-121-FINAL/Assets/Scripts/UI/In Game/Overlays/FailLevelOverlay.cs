using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailLevelOverlay : MonoBehaviour
{
    private void Start()
    {
        NeonRounds.instance?.gameData.GAME_FailLevel.AddListener(SetActive);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        NeonRounds.instance?.gameData.GAME_FailLevel.RemoveListener(SetActive);
    }

    void SetActive()
    {
        gameObject.SetActive(true);
    }
}
