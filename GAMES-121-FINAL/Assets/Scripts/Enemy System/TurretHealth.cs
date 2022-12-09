using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : EnemyHealth
{
    protected override void PreDeathEvent()
    {
        Instantiate(m_bundleDrop, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("Damaged");
    }
}
