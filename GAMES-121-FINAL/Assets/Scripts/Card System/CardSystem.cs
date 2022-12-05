using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSystem : MonoBehaviour
{
    #region Card Inventory Settings
    [Header("Card Inventory Settings")]
    //Cards and slots
    [SerializeField] static Card[] m_cards;
    static RectTransform[] m_cardSlots;
    static int m_slotUsedCount = 0;
    public static int SlotUsedCount 
    { 
        get { return m_slotUsedCount; } 
        set { m_slotUsedCount = value; }
    }

    //Selected card
    static int m_selectedCardIndex;

    //Start card
    [SerializeField] GameObject m_startBundle;
    #endregion

    #region Animation Settings
    [Header("Animation Settings")]
    [SerializeField] static float m_cardMoveDuration = 0.5f;
    #endregion

    void Start()
    {
        #region Initiate Card Slots
        m_cardSlots = new RectTransform[m_cards.Length];
        for (int i = 0; i < m_cards.Length; i++)
        {
            m_cardSlots[i] = m_cards[i].GetComponent<RectTransform>();
        }
        #endregion

        #region Initiate Bundle
        GameObject _newBundle = Instantiate(m_startBundle, Vector2.zero, Quaternion.identity);
        AddCard(_newBundle);
        m_selectedCardIndex = 0;
        #endregion
    }

    #region Card System Methods
    //Notes:
    //Card Deletion is handled in each Card object by setting itself inactive

    public static void AddCard(GameObject _newBundle)
    {
        //Check if slots are full. This step should be done before the method is called
        if (m_slotUsedCount >= m_cards.Length) return;

        //Create a new card at next available slot
        m_cards[m_slotUsedCount].CreateCard(_newBundle);

        //Add used slot count
        m_slotUsedCount++;
    }

    public static void SwitchCard()
    {
        //Set index to check
        int _potentialIndex;
        if (m_selectedCardIndex < m_cards.Length - 1) _potentialIndex = m_selectedCardIndex + 1;
        else _potentialIndex = 0;

        //Switch selected card
        if (m_cards[_potentialIndex].isActive)
        {
            m_cards[m_selectedCardIndex].SelectCard(false);
            m_selectedCardIndex = _potentialIndex;
            m_cards[m_selectedCardIndex].SelectCard(true);
        }
    }

    public static void UpdateCardPosition()
    {
        for (int i = 1; i < m_cards.Length; i++)
        {
            if (m_cards[i].isActive && !m_cards[i - 1].isActive)
            {
                //Get a copy of card in position i - 1
                Card _temp = m_cards[i - 1];

                //Move cards to new position (Swap position)
                m_cards[i].GetComponent<RectTransform>().DOAnchorPos(m_cardSlots[i - 1].anchoredPosition, m_cardMoveDuration);
                m_cards[i - 1].GetComponent<RectTransform>().anchoredPosition = m_cardSlots[i].anchoredPosition;

                //Update cards array
                m_cards[i - 1] = m_cards[i];
                m_cards[i] = _temp;
            }
        }
    }
    #endregion
}
