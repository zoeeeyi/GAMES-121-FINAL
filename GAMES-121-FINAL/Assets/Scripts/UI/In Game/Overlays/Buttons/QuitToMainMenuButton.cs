using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitToMainMenuButton : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        NeonRounds.instance.GameStateMachine.HandleExit(GameStateMachine.TriggerType.MenuButton);
    }
}
