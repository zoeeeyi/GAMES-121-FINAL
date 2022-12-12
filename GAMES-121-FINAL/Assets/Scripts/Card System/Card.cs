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
    GameObject bundle;

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
        bundle = _newBundle;
        WeaponParent _weapon = _newBundle.GetComponentInChildren<WeaponParent>();
        SkillParent _skill = _newBundle.GetComponentInChildren<SkillParent>();
        if (_weapon.isMeleeWeapon) m_ammoCount.text = "\u221E";
        else m_ammoCount.text =  _weapon.ammoCount.ToString();
        m_weaponName.text = _weapon.gameObject.name;
        m_SkillName.text = _skill.gameObject.name;

        //Bind this card to the weapon
        _weapon.card = this;

        //Activate this card
        m_background.SetActive(true);
        isActive = true;

        //Trigger Animation
        m_bgAnimator.SetTrigger("Add Card");
    }

    public void DeleteCard(bool _checkIfDeckEmpty = true)
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

        //Add start bundle back if all other cards are gone
        if (_checkIfDeckEmpty)
        {
            if (CardSystem.instance.slotUsedCount == 0)
            {
                CardSystem.instance.AddCard(CardSystem.instance.startBundle);
            }
        }


        //Reset animator
        //ResetAnimator();
    }

    public void DeleteCardAndBundle()
    {
        SelectCard(false);
        bundle.GetComponentInChildren<SkillParent>().SetToBeDisabled(true);
        DeleteCard(false);
    }

    public void SelectCard(bool _b)
    {
        if (_b)
        {
            m_bgAnimator.SetTrigger("Select");

            //Set the bundle back online
            if (bundle != null)
            {
                bundle.SetActive(true);
                foreach (Transform _child in bundle.transform)
                {
                    _child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            m_bgAnimator.SetTrigger("Deselect");

            //Set weapon and skill offline
            WeaponParent _weapon = bundle.GetComponentInChildren<WeaponParent>();
            if (_weapon != null)
            {
                _weapon.gameObject.SetActive(false);
                if (_weapon.ammoCount > 0 || _weapon.isMeleeWeapon)
                {
                    //If bullet count is 0, the skill will destroy itself when it's completed
                    //This step is for temporarily disable the weapon when switching
                    bundle.GetComponentInChildren<SkillParent>().SetToBeDisabled();
                }
            }
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

    public string GetWeaponName()
    {
        return m_weaponName.text;
    }
    #endregion
}
