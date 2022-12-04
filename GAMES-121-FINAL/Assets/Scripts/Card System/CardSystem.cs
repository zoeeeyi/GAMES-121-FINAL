using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSystem : MonoBehaviour
{
    [SerializeField] Card[] m_cards;
    RectTransform[] m_cardSlots;

    #region Animation Settings
    float m_cardMoveDuration = 0.5f;
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
    }

    public void UpdateCardPosition()
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
}
