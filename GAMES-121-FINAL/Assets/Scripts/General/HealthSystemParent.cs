using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthSystemParent : MonoBehaviour
{
    [SerializeField] bool m_immortal;
    [HideIf("m_immortal", true)]
    [SerializeField] protected int m_totalHealth;
    protected int m_currentHealth;

    protected virtual void Start()
    {
        m_currentHealth = m_totalHealth;
    }

    public virtual void TakeDamage()
    {
        if (m_immortal) return;

        if (m_currentHealth <= 0) return;
        m_currentHealth--;
        if (m_currentHealth <= 0) PreDeathEvent();
    }

    protected abstract void PreDeathEvent();
    protected abstract void DeathEvent();
}
