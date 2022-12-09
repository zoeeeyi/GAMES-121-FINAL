using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] List<string> m_targetTag;
    [SerializeField] ParticleSystem m_hitParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_targetTag.Contains(collision.tag))
        {
            //Visual effects
            if(m_hitParticle != null) Instantiate(m_hitParticle, transform.position, Quaternion.identity);
            CameraController.instance.CameraShake();

            //Apply damage
            collision.TryGetComponent<HealthSystemParent>(out HealthSystemParent _h);
            if (_h != null) _h.TakeDamage();
        }
        
        Destroy(gameObject);
    }
}
