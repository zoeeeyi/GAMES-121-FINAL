using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [HideInInspector] public bool isActive = false;

    public int onScreenOrder;

    [Header("Card Design")]
    [SerializeField] GameObject m_background;
    Animator m_bgAnimator;
    Canvas m_canvas; //for sorting order
    int m_currentSortOrder;
    public int currentSortOrder { get { return m_currentSortOrder; } set { m_currentSortOrder = value; } }

    [Header("Info Display")]
    [SerializeField] TextMeshProUGUI m_ammoCount;
    [SerializeField] TextMeshProUGUI m_weaponName;
    [SerializeField] TextMeshProUGUI m_SkillName;
    Image m_cardBackground;
    Sprite[] m_cardBgSprites;
    GameObject bundle;

    private void Awake()
    {
        //Fetch Components
        m_bgAnimator = m_background.GetComponent<Animator>();
        m_cardBackground = m_background.GetComponent<Image>();
        m_canvas = GetComponent<Canvas>();

        //Set background inactive when the game starts and player has no inventory
        m_background.SetActive(false);

        //Set other variables
        m_currentSortOrder = m_canvas.sortingOrder;
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

        //Update card background
        m_cardBgSprites = _weapon.weaponCardBackgrounds;
        if (m_cardBgSprites.Length > 0)
        {
            m_cardBackground.sprite = m_cardBgSprites[m_cardBgSprites.Length - 1];
        }
        else m_cardBackground.sprite = null;
        m_ammoCount.gameObject.SetActive(m_cardBgSprites.Length < 1); //Disable ammo count text if the ammo count is reflected through sprites

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
        m_cardBackground.sprite = null;
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
            m_canvas.sortingOrder = CardSystem.instance.totalSlots;

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
            m_canvas.sortingOrder = m_currentSortOrder;

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
        else
        {
            m_ammoCount.text = _c.ToString();
            if (m_cardBgSprites.Length > 1) m_cardBackground.sprite = m_cardBgSprites[_c - 1];
        }
    }

    public string GetWeaponName()
    {
        return m_weaponName.text;
    }
    #endregion
}
