using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : EnemyHealth
{
    [SerializeField] Transform m_cardDropPosition;

    protected override void Start()
    {
        base.Start();
        if (m_cardDropPosition == null) m_cardDropPosition = transform;
    }

    protected override void PreDeathEvent()
    {
        base.PreDeathEvent();
        if (m_cardDrop != null) Instantiate(m_cardDrop, m_cardDropPosition.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("Damaged");
        GetComponent<Collider2D>().enabled = false;
    }
}
