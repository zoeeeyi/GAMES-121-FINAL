using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthSystemParent : MonoBehaviour
{
    [SerializeField] protected int m_totalHealth;
    protected int m_currentHealth;

    protected void Start()
    {
        m_currentHealth = m_totalHealth;
    }

    public virtual void TakeDamage()
    {
        if (m_currentHealth <= 0) return;
        m_currentHealth--;
        if (m_currentHealth <= 0) PreDeathEvent();
    }

    protected abstract void PreDeathEvent();
    protected abstract void DeathEvent();
}
