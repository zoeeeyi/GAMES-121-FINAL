using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : SkillParent
{
    [SerializeField] float m_doubleJumpForce;
    [SerializeField] ForceMode2D m_forceMode;

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
        m_characterMovement.DoubleJump(m_doubleJumpForce, m_forceMode);
    }
}
