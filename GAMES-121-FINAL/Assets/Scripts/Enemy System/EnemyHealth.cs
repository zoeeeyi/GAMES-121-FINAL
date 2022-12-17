using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHealth : HealthSystemParent
{
    [SerializeField] protected GameObject m_cardDrop;
    protected override void PreDeathEvent()
    {
        gameObject.transform.parent = null;
        EnemyCounter.instance?.UpdateEnemyCount();
    }

    protected override void DeathEvent()
    {
        Destroy(gameObject);
    }
}
