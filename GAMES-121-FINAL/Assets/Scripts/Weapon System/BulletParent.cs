using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] string m_targetTag;
    [SerializeField] ParticleSystem m_hitParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == m_targetTag)
        {
            //Visual effects
            if(m_hitParticle != null) Instantiate(m_hitParticle, transform.position, Quaternion.identity);
            StartCoroutine(CameraController.instance.CameraShake());

            //Apply damage
            collision.TryGetComponent<HealthSystemParent>(out HealthSystemParent _h);
            if (_h != null) _h.TakeDamage();
        }
        Destroy(gameObject);
    }
}
