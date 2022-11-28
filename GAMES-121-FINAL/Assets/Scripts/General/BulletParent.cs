using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] string m_targetTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == m_targetTag)
        {
            collision.TryGetComponent<HealthSystemParent>(out HealthSystemParent _h);
            if (_h != null) _h.TakeDamage();
        }
        Destroy(gameObject);
    }
}
