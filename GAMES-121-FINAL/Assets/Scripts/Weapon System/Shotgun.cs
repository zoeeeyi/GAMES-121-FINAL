using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponParent
{
    [BoxGroup("Weapon Settings")]
    [SerializeField] float m_shootForce;
    [BoxGroup("Weapon Settings")] [Range(0, 45)] [SerializeField] int m_shotSpreadAngle;
    [BoxGroup("Weapon Settings")] [Range(0, 10)] [SerializeField] int m_oneShotBulletAmount;

    [BoxGroup("Visual")]
    [Range(0f, 1f)]
    [SerializeField] float m_cameraShakeStrength;

    [BoxGroup("Cursor Settings")] [SerializeField] Texture2D m_defaultCursor;
    [BoxGroup("Cursor Settings")] [SerializeField] Vector2 m_cursorOffset;
    [BoxGroup("Cursor Settings")] [SerializeField] CursorMode m_cursorMode;
    [BoxGroup("Cursor Settings")] [SerializeField] CursorAnimationClip m_shootCursorAnimation;

    private void OnEnable()
    {
        CursorController.instance?.SetDefaultCursor(m_defaultCursor, m_cursorOffset, m_cursorMode);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        CameraController.instance.CameraShake(m_cameraShakeStrength);
        CursorController.instance?.PlayCursorAnimation.Invoke(m_shootCursorAnimation);
        m_animator.SetTrigger("Shoot");
        m_audioManager.Play("Fire");

        GameObject[] _bullets = new GameObject[m_oneShotBulletAmount];
        //Get the angle for the first bullet
        float _rotation = m_aimEulerAngle - m_shotSpreadAngle * (m_oneShotBulletAmount - 1) / 2;
        for (int i = 0; i < m_oneShotBulletAmount; i++)
        {
            GameObject _bullet = Instantiate(m_bulletObject, m_firePoint.position, Quaternion.Euler(0, 0, _rotation));
            _bullet.GetComponent<Rigidbody2D>().AddForce(m_shootForce * _bullet.transform.right, ForceMode2D.Impulse);

            //Add angle for next bullet
            _rotation += m_shotSpreadAngle;
        }
    }
}
