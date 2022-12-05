using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : SkillParent
{
    [SerializeField] float m_hitPauseTime;

    //Dash by setting speed
    [SerializeField] float m_dashSpeed;
    [SerializeField] float m_dashTime;

    //Dash by setting force
/*    [SerializeField] float m_dashForce;
    [SerializeField] ForceMode2D m_dashMode;
    [SerializeField] float m_dashTime;
*/
    PlayerInput m_playerInput;

    protected override void Awake()
    {
        base.Awake();
        m_playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ExecuteSkill()
    {
        //Preparation
        m_playerInput.DisableMovementInput(true);
        m_characterMovement.ChangeGravityScale(0);
        StartCoroutine(HitPause());
        StartCoroutine(DashTime());

        //Dash
        Vector2 _dashDir = (m_bundledWeapon != null) ? - m_bundledWeapon.aimDir.normalized
            : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //by force
        //m_characterMovement.Dash(_inputDir, m_dashForce, m_dashMode);
        //by speed
        m_characterMovement.Dash(_dashDir, m_dashSpeed);
    }

    IEnumerator HitPause()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(m_hitPauseTime);
        Time.timeScale = 1;
    }

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(m_dashTime);
        m_playerInput.DisableMovementInput(false);

        //Finish Loop
        FinishEventLoop();
    }
}
