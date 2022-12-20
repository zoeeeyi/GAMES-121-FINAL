using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : WeaponParent
{
    #region Katana Properties
    [BoxGroup("Weapon Settings")]
    [SerializeField] Transform m_attackStartPoint;
    float m_circleCastLength;
    [BoxGroup("Weapon Settings")]
    [SerializeField] float m_attackRadius;
    [BoxGroup("Weapon Settings")]
    [SerializeField] LayerMask m_targetLayers;
    [BoxGroup("Weapon Settings")]
    [SerializeField] LayerMask m_obstacleCheckLayers;
    #endregion

    #region Visual Effects
    [BoxGroup("Visual")]
    [SerializeField] ParticleSystem m_hitParticle;
    [BoxGroup("Visual")]
    [SerializeField] ParticleSystem m_shieldParticle;
    #endregion

    #region State Machine
    bool state_damageDealt = false;
    #endregion

    Transform m_player;

    protected override void Start()
    {
        base.Start();

        m_player = GameObject.FindGameObjectWithTag("Player").transform;

        m_circleCastLength = (m_firePoint.position - m_attackStartPoint.position).magnitude;
    }

    private void OnDisable()
    {
        m_animator.SetTrigger("Reset");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (state_paused) return;
        if (Input.GetButtonDown("Fire")) Fire();
        if (Input.GetButtonUp("Fire") && state_attacking) Parry();
    }

    #region Action Methods
    protected override void Fire()
    {
        state_damageDealt = false;
        state_attacking = true;
        m_animator.SetTrigger("Strike");
        m_audioManager.Play("Slash");
    }

    void Parry()
    {
        m_shieldParticle.Play();
        m_animator.SetTrigger("Parry");
        state_attacking = false;
        //m_audioManager.Play("Parry");
    }

    //During attack animation, we need to check if Katana is in a wall
    public void InsideWallCheck()
    {
/*        RaycastHit2D _hit = Physics2D.Raycast(m_firePoint.position, m_player.position - m_firePoint.position, Mathf.Infinity, m_obstacleCheckLayers);
        if (_hit)
        {
            state_attacking = false;
            m_animator.SetTrigger("Reset");
        }
*/    }

    //Deal damage is triggered by animation
    void DealDamage()
    {
        if (state_damageDealt) return;
        Vector2 _circleCastDir = m_firePoint.position - m_attackStartPoint.position;
        RaycastHit2D[] _hit = Physics2D.CircleCastAll(m_attackStartPoint.position, m_attackRadius, _circleCastDir, m_circleCastLength, m_targetLayers);
        foreach (RaycastHit2D _h in _hit)
        {
            //Check if there's probably some obstacles in between
            RaycastHit2D _checkObstacle = Physics2D.Raycast(m_firePoint.position, _h.collider.ClosestPoint(m_firePoint.position) - (Vector2)m_firePoint.position, Mathf.Infinity, m_obstacleCheckLayers);
            if (_checkObstacle) continue;

            //Confirm damage dealt for this attack
            state_damageDealt = true;

            #region Deal Damage
            _h.transform.TryGetComponent<HealthSystemParent>(out HealthSystemParent _health);
            if (_health != null)
            {
                Instantiate(m_hitParticle, _h.transform.position, Quaternion.identity);
                _health.TakeDamage();
                CameraController.instance.CameraShake();
            }
            #endregion
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (m_firePoint != null)
        {
            Gizmos.DrawWireSphere(m_firePoint.position, m_attackRadius);
        }

        if (m_attackStartPoint != null)
        {
            Gizmos.DrawWireSphere(m_attackStartPoint.position, m_attackRadius);
        }
    }
}
