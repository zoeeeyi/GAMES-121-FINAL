using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    [HideInInspector] public bool isActive = false;

    [Header("Card Design")]
    [SerializeField] GameObject m_background;
    Animator m_bgAnimator;

    [Header("Info Display")]
    [SerializeField] TextMeshProUGUI m_ammoCount;
    [SerializeField] TextMeshProUGUI m_weaponName;
    [SerializeField] TextMeshProUGUI m_SkillName;
    GameObject m_bundle;

    private void Awake()
    {
        //Fetch Components
        m_bgAnimator = m_background.GetComponent<Animator>();

        //Set background inactive when the game starts and player has no inventory
        m_background.SetActive(false);
    }

    #region Utility Methods
    void ResetAnimator()
    {
        //Reset animator trigger
        m_bgAnimator.ResetTrigger("Select");
        m_bgAnimator.ResetTrigger("Deselect");
    }

    #endregion

    #region Creation, Deletion, Selection
    public void CreateCard(GameObject _newBundle)
    {
        //Update card info
        m_bundle = _newBundle;
        WeaponParent _weapon = _newBundle.GetComponentInChildren<WeaponParent>();
        SkillParent _skill = _newBundle.GetComponentInChildren<SkillParent>();
        m_ammoCount.text =  _weapon.bulletCount.ToString();
        m_weaponName.text = _weapon.gameObject.name;
        m_SkillName.text = _skill.gameObject.name;

        //Activate this card
        m_background.SetActive(true);
        isActive = true;

        //Trigger Animation
        m_bgAnimator.SetTrigger("Add Card");
    }

    public void DeleteCard()
    {
        //Check if slots are empty. This step should be done before the method is called
        if (CardSystem.instance.slotUsedCount <= 0) return;

        //De-activate the card
        m_background.SetActive(false);
        m_background.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        isActive = false;

        //Update card system
        CardSystem.instance.SwitchCard();
        CardSystem.instance.slotUsedCount--;
        CardSystem.instance.UpdateCardPosition();

        //Reset animator
        //ResetAnimator();
    }

    public void SelectCard(bool _b)
    {
        if (_b)
        {
            m_bgAnimator.SetTrigger("Select");

            //Set the bundle back online
            if (m_bundle != null)
            {
                m_bundle.SetActive(true);
                foreach (Transform _child in m_bundle.transform)
                {
                    _child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            m_bgAnimator.SetTrigger("Deselect");

            //Set weapon and skill offline
        }
    }
    #endregion

    #region Update Display Infomation
    public void SetAmmoCount(int _c)
    {
        //If ammo is used up, we delete this card
        if (_c <= 0) DeleteCard();
        else m_ammoCount.text = _c.ToString();
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
