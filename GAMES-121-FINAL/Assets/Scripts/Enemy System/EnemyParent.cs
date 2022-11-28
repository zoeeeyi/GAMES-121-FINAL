using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyParent : MonoBehaviour
{
    #region Attack Rounds Timer Variables
    [SerializeField] protected float m_timeBetweenRounds;
    protected float m_timeBetweenRoundsTimer;
    #endregion

    #region Player and Detection Variables
    [SerializeField] protected LayerMask m_targetLayer;
    protected bool m_detectionActivated = false;
    protected Transform m_player;
    #endregion

    // Start is called before the first frame update
    protected void Start()
    {
        #region Fetch Components
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        #endregion

        #region Set Values
        m_timeBetweenRoundsTimer = m_timeBetweenRounds;
        #endregion
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        #region Attact Target
        if (m_detectionActivated)
        {
            if (m_timeBetweenRoundsTimer > 0) m_timeBetweenRoundsTimer -= Time.deltaTime;
            else
            {
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, m_player.position - transform.position, Mathf.Infinity, m_targetLayer);
                if (_hit)
                {
                    ExecuteAttack();
                    m_timeBetweenRoundsTimer = m_timeBetweenRounds;
                }
            }
        }
        #endregion
    }

    public void DetectionSwitch(bool _bool)
    {
        m_detectionActivated = _bool;
    }

    protected abstract void ExecuteAttack();
}
