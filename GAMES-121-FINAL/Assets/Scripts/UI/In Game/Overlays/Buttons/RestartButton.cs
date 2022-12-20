using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        NeonRounds.instance.GameStateMachine.HandleRestartLevel(GameStateMachine.TriggerType.MenuButton);
    }
}
