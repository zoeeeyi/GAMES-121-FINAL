using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress Tracker", menuName = "Scriptable Objects/Progress Tracker")]
public class ProgressTracker : ScriptableObject
{
    //Speedrun Mode Timer Settings
    [SerializeField] float m_totalGameTime;
    [SerializeField] float m_currentSessionRemainingTime;

    //Gem Settings
    [SerializeField] int m_gemCollected;
}
