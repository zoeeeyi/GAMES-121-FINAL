using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponParent
{
    [SerializeField] float m_bulletForce;

    [Header("Visual Effect Settings")]
    [Range(0f, 1f)]
    [SerializeField] float m_cameraShakeStrength;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        CameraController.instance.CameraShake(m_cameraShakeStrength);
        m_animator.SetTrigger("Shoot");
        GameObject _bullet = Instantiate(m_bullet, m_bulletPoint.position, m_weaponSprite.transform.rotation);
        _bullet.GetComponent<Rigidbody2D>().AddForce(m_bulletForce * _bullet.transform.right, ForceMode2D.Impulse);
    }
}
