using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] List<string> m_targetTag;
    [SerializeField] List<string> m_exceptionTag;
    [SerializeField] ParticleSystem m_hitTargetParticle;
    [SerializeField] ParticleSystem m_otherHitParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_targetTag.Contains(collision.tag))
        {
            //Visual effects
            Instantiate(m_hitTargetParticle, transform.position, Quaternion.identity);
            CameraController.instance.CameraShake();

            //Apply damage
            collision.TryGetComponent<HealthSystemParent>(out HealthSystemParent _h);
            if (_h != null) _h.TakeDamage();
        } else if (m_otherHitParticle != null) Instantiate(m_otherHitParticle, transform.position, Quaternion.identity);

        if (m_exceptionTag.Contains(collision.tag)) return;
        Destroy(gameObject);
    }
}
