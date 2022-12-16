using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : EnemyHealth
{
    protected override void PreDeathEvent()
    {
        Instantiate(m_cardDrop, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("Damaged");
        GetComponent<Collider2D>().enabled = false;
    }
}
