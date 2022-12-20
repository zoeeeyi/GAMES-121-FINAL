using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyParent : MonoBehaviour
{
    [Header("General Settings")]
    #region Attack Rounds Timer Variables
    [SerializeField] protected float m_timeBetweenRounds;
    protected float m_timeBetweenRoundsTimer = 0;
    #endregion

    #region Player and Detection Variables
    [SerializeField] protected LayerMask m_raycastLayer;
    [SerializeField] protected Transform m_aimDevice;
    protected Transform m_target;
    protected Vector2 m_targetDir = Vector2.zero;
    #endregion

    #region States Variables
    protected bool state_paused = false;
    protected bool state_detectionActivated = false;
    protected bool state_seeTarget = false;
    protected bool state_attacking = false;
    void Pause() { state_paused = true; }
    void Unpause() { state_paused = false; }
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        #region Fetch Components and set values
        m_target = GameObject.FindGameObjectWithTag("Player").transform;

        if (m_aimDevice == null) m_aimDevice = transform;
        #endregion

        #region Event Subscription
        NeonRounds.instance.gameData.GAME_ContinueLevel.AddListener(Unpause);
        NeonRounds.instance.gameData.GAME_PauseLevel.AddListener(Pause);
        #endregion
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (state_paused) return;

        #region Look for target
        if (state_detectionActivated)
        {
            //Start looking for target
            m_targetDir = m_target.position - m_aimDevice.position;
            RaycastHit2D _hit = Physics2D.Raycast(m_aimDevice.position, m_targetDir, Mathf.Infinity, m_raycastLayer);

            //if the raycast directly hits the target, the enemy see the target
            if (_hit.collider.tag == "Player")
            {
                state_seeTarget = true;
                //Count down timer if the enemy can see the target
                if (m_timeBetweenRoundsTimer > 0) m_timeBetweenRoundsTimer 
                        = Mathf.Clamp(m_timeBetweenRoundsTimer, 0, m_timeBetweenRoundsTimer - Time.deltaTime);
            }
            else
            {
                state_seeTarget = false;
                m_timeBetweenRoundsTimer = 0;
            }
        }
        #endregion
    }

    protected virtual void OnDestroy()
    {
        NeonRounds.instance.gameData.GAME_ContinueLevel.RemoveListener(Unpause);
        NeonRounds.instance.gameData.GAME_PauseLevel.RemoveListener(Pause);
    }

    #region Utility Methods
    private void ResetValue()
    {
        state_seeTarget = false;
        m_timeBetweenRoundsTimer = 0;
    }

    public void DetectionSwitch(bool _bool)
    {
        state_detectionActivated = _bool;
        if (_bool) ResetValue();
    }
    #endregion

    protected abstract void ExecuteAttack();

    protected virtual void FinishAttack()
    {
        state_attacking = false;
        m_timeBetweenRoundsTimer = m_timeBetweenRounds;
    }
}
