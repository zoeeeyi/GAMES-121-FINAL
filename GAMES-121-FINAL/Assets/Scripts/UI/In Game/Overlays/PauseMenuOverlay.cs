using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOverlay : MonoBehaviour
{
    private void Start()
    {
        NeonRounds.instance?.gameData.GAME_ContinueLevel.AddListener(SetInactive);
        NeonRounds.instance?.gameData.GAME_PauseLevel.AddListener(SetActive);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        NeonRounds.instance?.gameData.GAME_ContinueLevel.RemoveListener(SetInactive);
        NeonRounds.instance?.gameData.GAME_PauseLevel.RemoveListener(SetActive);
    }

    void SetActive()
    {
        if (NeonRounds.instance?.currentGameState == NeonRounds.GameState.InPauseMenu) gameObject.SetActive(true);
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }

}
