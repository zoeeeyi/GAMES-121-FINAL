using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryTest_Temp : MonoBehaviour
{
    [SerializeField] List<GameObject> combos = new List<GameObject>();
    [SerializeField] TextMeshProUGUI m_weaponName;
    [SerializeField] TextMeshProUGUI m_bulletCount;
    int m_currentIndex = 0;
    WeaponParent m_currentWeapon;

    private void Start()
    {
        for (int i = 0; i < combos.Count; i++)
        {
            if (i != m_currentIndex)
            {
                combos[i].SetActive(false);
            }
        }
        m_currentIndex = 0;
        m_currentWeapon = combos[0].GetComponentInChildren<WeaponParent>();
        UpdateWeaponNameUI();
    }

    private void Update()
    {
        if (combos.Count <= 0) return;

        int _bulletCount = m_currentWeapon.bulletCount;
        if (_bulletCount <= 0)
        {
            if (combos.Count > 1)
            {
                SwitchWeapon();
            }
            else
            {
                combos[m_currentIndex].SetActive(false);
                m_weaponName.text = "N/A";
                m_bulletCount.text = "0";
                return;
            }
        }
        m_bulletCount.text = m_currentWeapon.bulletCount.ToString();

        if (Input.GetKeyDown(KeyCode.Q) && combos.Count > 1) SwitchWeapon();
    }

    public void SwitchWeapon()
    {
        combos[m_currentIndex].gameObject.SetActive(false);
        m_currentIndex = (m_currentIndex + 1 < combos.Count) ? m_currentIndex + 1 : 0;
        m_currentWeapon = combos[m_currentIndex].GetComponentInChildren<WeaponParent>();
        UpdateWeaponNameUI();
        combos[m_currentIndex].gameObject.SetActive(true);
    }

    public void UpdateWeaponNameUI()
    {
        m_weaponName.text = m_currentWeapon.weaponName;
    }
}