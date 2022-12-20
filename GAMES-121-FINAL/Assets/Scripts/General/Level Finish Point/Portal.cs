using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] string m_nextLevel = "Transition Scene";
    [SerializeField] NeonRounds.GameState m_nextGameState = NeonRounds.GameState.InTransitionMenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") NeonRounds.instance?.LoadLevel(m_nextLevel, m_nextGameState);
    }
}
