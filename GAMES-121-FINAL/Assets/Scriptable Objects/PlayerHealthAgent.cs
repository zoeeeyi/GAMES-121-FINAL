using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Player Health Agent", menuName = "Scriptable Objects/Player Health Agent")]
public class PlayerHealthAgent : ScriptableObject
{
    public UnityEvent<int> UpdatePlayerHealth = new UnityEvent<int>();
}
