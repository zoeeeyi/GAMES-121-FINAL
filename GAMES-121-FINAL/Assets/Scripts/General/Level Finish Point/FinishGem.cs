using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGem : MonoBehaviour
{
    [SerializeField] string m_nextLevel;
    [SerializeField] EnemyCounterAgent m_enemyCounterAgent;
    [SerializeField] float m_freezePlayerTime = 2;
    bool m_gotGem = false;
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_enemyCounterAgent.UpdateEnemyCount.AddListener(UnlockFinishGem);
    }

    //Triggers winning condition
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !m_gotGem)
        {
            m_gotGem = true;
            if (NeonRounds.instance != null)
            {
                int _nextLevelIndex = NeonRounds.instance.gameData.levelDic[NeonRounds.instance.gameData.currentLevel] + 1;
                if (_nextLevelIndex >= NeonRounds.instance.gameData.levelList.Length) _nextLevelIndex--;
                NeonRounds.instance?.WinLevel(NeonRounds.instance.gameData.levelList[_nextLevelIndex]);
            }
            MovementInput _player = collision.GetComponent<MovementInput>();
            _player.DisableMovementInput(true, true);
            StartCoroutine(UnfreezePlayer(_player));
            m_animator.SetTrigger("Get Gem");
        }
    }

    //Block player from finishing level
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_animator.SetTrigger("Block");
        }
    }

    #region Utility Methods
    void UnlockFinishGem(int _i = 1) //add an int to fullfill requirement to be added to Update Enemy Count Event
    {
        if (m_enemyCounterAgent.enemyCount <= 0) GetComponent<Collider2D>().isTrigger = true;
    }

    public void ShakeCamera(float _intensity)
    {
        CameraController.instance?.CameraShake(_intensity, 2);
    }

    IEnumerator UnfreezePlayer(MovementInput _player)
    {
        yield return new WaitForSeconds(m_freezePlayerTime);
        _player.DisableMovementInput(false);
    }
    #endregion
}
