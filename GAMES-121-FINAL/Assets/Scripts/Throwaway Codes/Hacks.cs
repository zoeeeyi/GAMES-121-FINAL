using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    static Hacks instance;
    [SerializeField] static GameObject m_bundle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(this);
        }
    }

    //These are the greatest hacks
    void Update()
    {
        if (NeonRounds.instance?.currentGameState == NeonRounds.GameState.InGame || NeonRounds.instance?.currentGameState == NeonRounds.GameState.InMainMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale > 1) Time.timeScale = 1;
                else Time.timeScale = 3;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                GameObject.Find("Player").GetComponent<PlayerHealth>().immortal = true;
            }

            if (Input.GetKeyDown(KeyCode.RightBracket)) GameObject.Find("Player").transform.localScale *= 2;
            if (Input.GetKeyDown(KeyCode.LeftBracket)) GameObject.Find("Player").transform.localScale /= 2;

            if (Input.GetKeyDown(KeyCode.N))
            {
                CardSystem.instance?.AddCard(m_bundle);
            }

        }
    }
}
