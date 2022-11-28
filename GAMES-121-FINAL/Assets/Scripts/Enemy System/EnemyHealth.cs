using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystemParent
{
    protected override void OnDeathEvent()
    {
        Destroy(gameObject);
    }
}
