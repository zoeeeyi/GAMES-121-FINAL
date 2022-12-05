using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : SkillParent
{
    //By speed
    [SerializeField] float m_doubleJumpSpeed;
    [SerializeField] float m_doubleJumpTime;

/*    //By force
    [SerializeField] float m_doubleJumpForce;
    [SerializeField] ForceMode2D m_forceMode;
*/
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ExecuteSkill()
    {
        //Preparation
        m_characterMovement.DisableGravity(true);
        m_characterMovement.DoubleJump(m_doubleJumpSpeed);
        //m_characterMovement.DoubleJump(m_doubleJumpForce, m_forceMode);

        StartCoroutine(DoubleJumpTime());
    }

    IEnumerator DoubleJumpTime()
    {
        yield return new WaitForSeconds(m_doubleJumpTime);
        m_characterMovement.DisableGravity(false);

        //Finish Loop
        FinishEventLoop();
    }
}
