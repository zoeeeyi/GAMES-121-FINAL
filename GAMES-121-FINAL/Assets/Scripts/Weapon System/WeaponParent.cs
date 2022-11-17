using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponParent : MonoBehaviour
{
    [SerializeField] protected int m_ammoCount;
    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            Fire();
        }
    }
    protected abstract void Fire();
}
