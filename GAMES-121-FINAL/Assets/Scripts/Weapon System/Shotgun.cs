using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponParent
{
    [BoxGroup("Weapon Settings")]
    [SerializeField] float m_shootForce;

    [BoxGroup("Visual")]
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
        m_audioManager.Play("Fire");
        GameObject _bullet = Instantiate(m_bulletObject, m_firePoint.position, m_weaponSprite.transform.rotation);
        _bullet.GetComponent<Rigidbody2D>().AddForce(m_shootForce * _bullet.transform.right, ForceMode2D.Impulse);
    }
}
