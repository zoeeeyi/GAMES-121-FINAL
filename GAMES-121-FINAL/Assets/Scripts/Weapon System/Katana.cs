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

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetButtonUp("Fire")) Fire();
    }

    protected override void Fire()
    {
        Collider2D[] _hit = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, m_targetLayers);
        foreach (Collider2D _h in _hit)
        {
            #region Deal Damage
            _h.TryGetComponent<HealthSystemParent>(out HealthSystemParent _health);
            if (_h != null) _health.TakeDamage();
            #endregion
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_attackPoint != null)
        {
            Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRadius);
        }
    }
}
