using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystemParent
{
    [SerializeField] GameObject m_bundleDrop;
    protected override void OnDeathEvent()
    {
        Instantiate(m_bundleDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
