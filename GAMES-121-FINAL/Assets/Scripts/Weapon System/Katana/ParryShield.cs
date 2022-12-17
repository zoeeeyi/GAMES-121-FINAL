using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryShield : MonoBehaviour
{
    [SerializeField] List<string> m_targetTag;
    [SerializeField] ParticleSystem m_hitTargetParticle;
    [SerializeField] Rigidbody2D m_parryBullet;
    [SerializeField] float m_parryVelocityMult;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Mathf.Sign(collision.transform.position.x - transform.position.x) == Mathf.Sign(transform.right.x))
        {
            if (m_targetTag.Contains(collision.tag))
            {
                //Create counter bullet
                collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb);
                if (_rb != null)
                {
                    int _xVelocityMult = (int) (Mathf.Sign(transform.right.x) * Mathf.Sign(_rb.velocity.x));
                    Vector2 _parryVelocity = new Vector2(_xVelocityMult * _rb.velocity.x, -_rb.velocity.y);
                    float _parryAngle = Mathf.Atan2(_parryVelocity.y, _parryVelocity.x) * Mathf.Rad2Deg;
                    Quaternion _parryRotation = Quaternion.Euler(0, 0, _parryAngle);

                    Rigidbody2D _newBullet = Instantiate(m_parryBullet, collision.transform.position, _parryRotation);
                    _newBullet.velocity = _parryVelocity * m_parryVelocityMult;
                }

                //Visual effects
                Instantiate(m_hitTargetParticle, collision.transform.position, Quaternion.identity);
                CameraController.instance.CameraShake(0.2f);
                Destroy(collision.gameObject);
            }
        }
    }

}
