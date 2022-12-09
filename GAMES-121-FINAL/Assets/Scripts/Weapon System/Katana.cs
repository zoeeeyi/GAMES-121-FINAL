using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : WeaponParent
{
    #region Katana Properties
    [Header("Katana Properties")]
    [SerializeField] Transform m_attackPoint;
    [SerializeField] float m_attackRadius;
    [SerializeField] LayerMask m_targetLayers;
    #endregion

    #region Visual Effects
    [SerializeField] ParticleSystem m_hitParticle;
    #endregion

    #region State Machine
    bool state_damageDealt = false;
    #endregion

    private void OnDisable()
    {
        m_animator.SetTrigger("Reset");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetButtonDown("Fire")) Fire();
        if (Input.GetButtonUp("Fire")) Parry();
    }

    #region Action Methods
    protected override void Fire()
    {
        state_damageDealt = false;
        m_animator.SetTrigger("Strike");
    }

    void Parry()
    {
        m_animator.SetTrigger("Parry");
    }

    //Deal damage is triggered by animation
    void DealDamage()
    {
        if (state_damageDealt) return;
        Collider2D[] _hit = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, m_targetLayers);
        if (_hit.Length > 0) state_damageDealt = true;
        foreach (Collider2D _h in _hit)
        {
            #region Deal Damage
            _h.TryGetComponent<HealthSystemParent>(out HealthSystemParent _health);
            if (_h != null)
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
        if (m_attackPoint != null)
        {
            Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRadius);
        }
    }
}
