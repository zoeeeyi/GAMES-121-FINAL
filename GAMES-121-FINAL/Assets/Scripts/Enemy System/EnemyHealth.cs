using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHealth : HealthSystemParent
{
    [SerializeField] protected GameObject m_bundleDrop;
    protected override abstract void PreDeathEvent();

    protected override void DeathEvent()
    {
        Destroy(gameObject);
    }
}
