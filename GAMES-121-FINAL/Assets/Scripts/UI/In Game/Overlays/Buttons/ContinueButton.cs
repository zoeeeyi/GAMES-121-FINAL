using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void ContinueLevel()
    {
        NeonRounds.instance.GameStateMachine.HandleExit(GameStateMachine.TriggerType.Key);
    }
}
