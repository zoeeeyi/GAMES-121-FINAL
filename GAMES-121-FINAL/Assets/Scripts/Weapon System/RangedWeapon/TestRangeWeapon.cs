using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeWeapon : RangeWeaponParent
{
    [SerializeField] float m_bulletForce;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        GameObject _bullet = Instantiate(m_bullet, m_bulletPoint.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().AddForce(m_bulletForce * transform.right, ForceMode2D.Impulse);
        if (m_toBeDestroyed) Destroy(gameObject);
    }

    #region Destroy
    public override void SetToBeDestroyed()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
