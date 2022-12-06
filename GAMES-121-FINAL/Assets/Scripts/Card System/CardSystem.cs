using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    public static CardSystem instance;
    #region Card Inventory Settings
    [Header("Card Inventory Settings")]
    //Cards and slots
    [SerializeField] Card[] m_cards;
    [SerializeField] Image m_cardSlotImage;
    [SerializeField] RectTransform m_cardSlotHolder;
    Vector3[] m_cardSlots;
    int m_slotUsedCount = 0;
    public int slotUsedCount 
    { 
        get { return m_slotUsedCount; } 
        set { m_slotUsedCount = value; }
    }

    //Selected card
    int m_selectedCardIndex;

    //Start card
    [SerializeField] GameObject m_startBundle;
    [SerializeField] GameObject m_testBundle;
    #endregion

    #region Animation Settings
    [Header("Animation Settings")]
    [SerializeField] static float m_cardMoveDuration = 0.5f;
    #endregion    

    private void Awake()
    {
/*        //Don't destroy on load
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
*/
        //Destroy on load
        instance = this;
    }

    void Start()
    {
        #region Initiate Card Slots
        m_cardSlots = new Vector3[m_cards.Length];
        for (int i = 0; i < m_cards.Length; i++)
        {
            Vector3 _pos = m_cards[i].GetComponent<RectTransform>().anchoredPosition;
            m_cardSlots[i] = _pos;

            //Instantiate Card Slot Image
            Image _new = Instantiate(m_cardSlotImage, Vector2.zero, Quaternion.identity, m_cardSlotHolder);
            _new.rectTransform.anchoredPosition = _pos + new Vector3(0, 0, -1);
        }
        #endregion

        #region Initiate Bundle
        m_selectedCardIndex = 0;
        AddCard(m_startBundle);
        #endregion
    }

    private void Update()
    {
        #region Input
        if (Input.GetButtonDown("Next Weapon"))
        {
            SwitchCard(-1);
        }

        if (Input.GetButtonDown("Previous Weapon"))
        {
            SwitchCard(-2);
        }
        #endregion

        #region More Inputs (These are for testing purposes)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCard(3);

        if (Input.GetKeyDown(KeyCode.N))
        {
            AddCard(m_testBundle);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_cards[m_selectedCardIndex].DeleteCard();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            m_cards[0].DeleteCard();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            m_cards[1].DeleteCard();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            m_cards[2].DeleteCard();
        }

        #endregion
    }

    #region Card System Methods
    //Notes:
    //Card Deletion is handled in each Card object by setting itself inactive

    public bool AddCard(GameObject _newBundle)
    {
        //Check if slots are full. This step should be done before the method is called
        if (m_slotUsedCount >= m_cards.Length) return false;

        //Instantiate gameobject
        GameObject _new = Instantiate(_newBundle, Vector2.zero, Quaternion.identity);

        //Create a new card at next available slot
        m_cards[m_slotUsedCount].CreateCard(_new);

        //Set it inactive if there are other cards
        if (m_slotUsedCount != m_selectedCardIndex) _new.SetActive(false);
        else m_cards[m_slotUsedCount].SelectCard(true);

        //Add used slot count
        m_slotUsedCount++;

        return true;
    }

    public void SwitchCard()
    {
        if (m_selectedCardIndex > 0) SwitchCard(-2);
        else SwitchCard(-1);
    }

    public void SwitchCard(int _index)
    {
        //Don't do this unless we have more than 1 card
        if (m_slotUsedCount <= 1) return;
        
        //Set index to check
        int _potentialIndex = new int();

        //Choose specific card
        if (_index >= 1 && _index <= m_slotUsedCount) _potentialIndex = _index - 1;
        //Choose Next card
        else if (_index == -1)
        {
            if (m_selectedCardIndex < m_slotUsedCount - 1) _potentialIndex = m_selectedCardIndex + 1;
            else _potentialIndex = 0;
        }
        //Choose Previous card
        else if (_index == -2)
        {
            if (m_selectedCardIndex > 0) _potentialIndex = m_selectedCardIndex - 1;
            else _potentialIndex = m_slotUsedCount - 1;
        }
        else return;

        //Switch selected card
        if (_potentialIndex == m_selectedCardIndex) return;
        if (m_cards[_potentialIndex].isActive)
        {
            m_cards[m_selectedCardIndex].SelectCard(false);
            m_selectedCardIndex = _potentialIndex;
            m_cards[m_selectedCardIndex].SelectCard(true);
        }
    }

    public void UpdateCardPosition()
    {
        for (int i = 1; i < m_cards.Length; i++)
        {
            if (m_cards[i].isActive && !m_cards[i - 1].isActive)
            {
                //Change selected card index if applies
                if (m_selectedCardIndex == i) m_selectedCardIndex = i - 1;

                //Get a copy of card in position i - 1
                Card _temp = m_cards[i - 1];

                //Stop existing Dotween operations
                DOTween.Kill(m_cards[i].GetComponent<RectTransform>());
                DOTween.Kill(m_cards[i - 1].GetComponent<RectTransform>());

                //Move cards to new position (Swap position)
                m_cards[i].GetComponent<RectTransform>().DOAnchorPos(m_cardSlots[i - 1], m_cardMoveDuration);
                m_cards[i - 1].GetComponent<RectTransform>().anchoredPosition = m_cardSlots[i];

                //Update cards array
                m_cards[i - 1] = m_cards[i];
                m_cards[i] = _temp;
            }
        }
    }
    #endregion
}
