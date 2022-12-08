using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyParent
{
    #region Bullet Settings
    [Header("Bullet Settings")]
    [SerializeField] private float m_bulletForce;
    [SerializeField] GameObject m_bullet;
    [SerializeField] Transform m_bulletPoint;
    #endregion

    #region More Aiming Settings
    [Header("More Aiming Settings")]
    [Range(0, 45)]
    [SerializeField] int m_aimErrorRange;
    [Range(0f, 1f)]
    [SerializeField] float m_turretRotationSmoothTime;
    float m_turretRotationSmoothV;
    SpriteRenderer m_aimDeviceSprite;
    Animator m_animator;
    #endregion

    protected override void Start()
    {
        base.Start();

        #region Fetch Components
        m_animator = GetComponent<Animator>();
        m_aimDeviceSprite = m_aimDevice.GetComponent<SpriteRenderer>();
        #endregion
    }

    protected override void Update()
    {
        #region State Machine
        if (state_attacking) return;
        #endregion

        base.Update();

        #region Aimming
        if (state_seeTarget)
        {
            //Flip sprite Y to make sure aim scope is facing upwards
            m_aimDeviceSprite.flipY = (m_targetDir.x < 0);

            //Rotate turret
            float _rotationZ = Mathf.Atan2(m_targetDir.y, m_targetDir.x) * Mathf.Rad2Deg;
            if (m_timeBetweenRoundsTimer <= 0)
            {
                float _difference = Mathf.Abs(_rotationZ - m_aimDevice.eulerAngles.z);
                //If the target is within some error, start attack
                if (_difference <= m_aimErrorRange || (360 - _difference) <= m_aimErrorRange) StartAttack();
            }
            //Otherwise, keep rotating the turret
            _rotationZ = Mathf.SmoothDampAngle(m_aimDevice.eulerAngles.z, _rotationZ, ref m_turretRotationSmoothV, m_turretRotationSmoothTime);
            m_aimDevice.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }
        #endregion
    }

    private void StartAttack()
    {
        state_attacking = true;
        m_animator.SetTrigger("Shoot");
    }

    protected override void ExecuteAttack()
    {
        //Shoot Bullet
        if (m_target == null) return;
        GameObject _bullet = Instantiate(m_bullet, m_bulletPoint.position, m_aimDevice.rotation);
        _bullet.GetComponent<Rigidbody2D>().AddForce(m_bulletForce * _bullet.transform.right, ForceMode2D.Impulse);
    }
}
