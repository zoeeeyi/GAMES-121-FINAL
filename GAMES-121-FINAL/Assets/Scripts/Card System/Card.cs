using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public bool isActive = false;

    [SerializeField] GameObject m_background;
    Animator m_bgAnimator;

    [Header("Info Display")]
    [SerializeField] TextMeshProUGUI m_ammoCount;
    [SerializeField] TextMeshProUGUI m_weaponName;
    [SerializeField] TextMeshProUGUI m_SkillName;

    private void Awake()
    {
        //Fetch Components
        m_bgAnimator = m_background.GetComponent<Animator>();

        //Set background inactive when the game starts and player has no inventory
        m_background.SetActive(false);
    }

    #region Creation, Deletion, Selection
    public void CreateCard(int _ammoCount, string _weaponName, string _skillName)
    {
        //Update display info
        m_ammoCount.text = _ammoCount.ToString();
        m_weaponName.text = _weaponName;
        m_SkillName.text = _skillName;

        m_background.SetActive(true);
        isActive = true;
    }

    public void DeleteCard()
    {
        m_background.SetActive(false);
        isActive = false;
    }

    public void SetCardHighlighted(bool _b)
    {
        if (_b) m_bgAnimator.SetTrigger("Select");
        else m_bgAnimator.SetTrigger("Deselect");
    }
    #endregion

    #region Update Display Infomation
    public void SetAmmoCount(int _c)
    {
        m_ammoCount.text = _c.ToString();
    }

    public void SetWeaponName(string _s)
    {
        m_weaponName.text = _s;
    }

    public void SetSkillName(string _s)
    {
        m_SkillName.text = _s;
    }
    #endregion
}
