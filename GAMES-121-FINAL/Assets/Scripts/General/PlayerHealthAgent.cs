using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerHealthAgent", menuName = "Scriptable Objects/Player Health Agent")]
public class PlayerHealthAgent : ScriptableObject
{
    [HideInInspector] public UnityEvent<int> UpdatePlayerHealth = new UnityEvent<int>();
}
