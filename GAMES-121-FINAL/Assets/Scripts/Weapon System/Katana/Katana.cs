using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : WeaponParent
{
    #region Katana Properties
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
    }

    private void OnDisable()
    {
        m_animator.SetTrigger("Reset");
    }

    // Update is called once per frame
    protected override void Update()
    {
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
        RaycastHit2D _hit = Physics2D.Raycast(m_firePoint.position, m_player.position - m_firePoint.position, Mathf.Infinity, m_obstacleCheckLayers);
        if (_hit)
        {
            Debug.Log("Inside Wall");
            state_attacking = false;
            m_animator.SetTrigger("Reset");
        }
    }

    //Deal damage is triggered by animation
    void DealDamage()
    {
        if (state_damageDealt) return;
        Collider2D[] _hit = Physics2D.OverlapCircleAll(m_firePoint.position, m_attackRadius, m_targetLayers);
        foreach (Collider2D _h in _hit)
        {
            //Check if there's probably some obstacles in between
            RaycastHit2D _checkObstacle = Physics2D.Raycast(m_firePoint.position, _h.ClosestPoint(m_firePoint.position) - (Vector2)m_firePoint.position, Mathf.Infinity, m_obstacleCheckLayers);
            if (_checkObstacle) continue;

            //Confirm damage dealt for this attack
            state_damageDealt = true;

            #region Deal Damage
            _h.TryGetComponent<HealthSystemParent>(out HealthSystemParent _health);
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
    }
}
