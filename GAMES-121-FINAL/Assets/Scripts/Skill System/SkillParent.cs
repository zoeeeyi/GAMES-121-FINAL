using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillParent : MonoBehaviour
{
    protected RangeWeaponParent m_bundledWeapon;
    protected CharacterMovement m_characterMovement;

    protected virtual void Awake()
    {
        m_characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        //Try to find bundled weapon
        if (transform.parent != null) m_bundledWeapon = transform.parent.GetComponentInChildren<RangeWeaponParent>();
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            if (m_bundledWeapon?.bulletCount > 0) ExecuteSkill();
            else ExecuteSkill();
        }
    }

    protected virtual void DestroyEvent()
    {
        if (m_bundledWeapon?.bulletCount <= 0)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject);
            GameObject.Find("Simple Inventory").GetComponent<SimpleInventory>().ChangeActiveBundle();
            Destroy(gameObject);
        }
    }

    protected abstract void ExecuteSkill();
}
