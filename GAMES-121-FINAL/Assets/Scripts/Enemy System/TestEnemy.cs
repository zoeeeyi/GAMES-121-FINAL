using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyParent
{
    [SerializeField] private float m_bulletForce;
    [SerializeField] GameObject m_bullet;

    protected override void ExecuteAttack()
    {
        Vector2 _targetPosition;
        if (m_player != null) _targetPosition = m_player.position;
        else return;
        GameObject _bullet = Instantiate(m_bullet, (Vector2)transform.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().AddForce(m_bulletForce * (_targetPosition - (Vector2)transform.position).normalized, ForceMode2D.Impulse);
    }
}
