using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryShield : MonoBehaviour
{
    [SerializeField] List<string> m_targetTag;
    [SerializeField] ParticleSystem m_hitTargetParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Mathf.Sign(collision.transform.position.x - transform.position.x) == Mathf.Sign(transform.right.x))
        {
            if (m_targetTag.Contains(collision.tag))
            {
                //Visual effects
                Instantiate(m_hitTargetParticle, collision.transform.position, Quaternion.identity);
                CameraController.instance.CameraShake(0.2f);
                Destroy(collision.gameObject);
            }
        }
    }

}
