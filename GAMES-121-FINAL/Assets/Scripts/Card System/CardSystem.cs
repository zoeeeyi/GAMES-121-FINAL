using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CardSystem : MonoBehaviour
{
    public static CardSystem instance;
    #region Card Inventory Settings
    [Header("Card Inventory Settings")]
    //Cards and slots
    [SerializeField] Card[] m_cards;
    [SerializeField] Image m_cardSlotImage;
    [SerializeField] RectTransform m_cardSlotHolder;
    CardSlot[] m_cardSlots;
    struct CardSlot
    {
        public Vector2 st_slotPos;
        public Quaternion st_slotRotation;
        public int st_sortingOrder;

        public CardSlot(Vector2 _pos, Quaternion _rotation, int _order)
        {
            st_slotPos = _pos;
            st_slotRotation = _rotation;
            st_sortingOrder = _order;
        }
    }

    public int totalSlots { get; private set; }
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
    public GameObject startBundle { get { return m_startBundle; } }
    [SerializeField] GameObject m_testBundle;
    #endregion

    #region Animation Settings
    [Header("Animation Settings")]
    [SerializeField] static float m_cardMoveDuration = 0.5f;
    [SerializeField] bool m_centerIfOnlyOneCard = false;
    #endregion

    #region State Controll
    bool state_paused;
    void Pause() { state_paused = true; }
    void Unpause() { state_paused = false; }
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
        #region Event Subscription
        NeonRounds.instance.gameData.GAME_ContinueLevel.AddListener(Unpause);
        NeonRounds.instance.gameData.GAME_PauseLevel.AddListener(Pause);
        #endregion

        #region Initiate Card Slots
        totalSlots = m_cards.Length;
        m_cardSlots = new CardSlot[m_cards.Length];
        for (int i = 0; i < m_cards.Length; i++)
        {
            RectTransform _rect = m_cards[i].GetComponent<RectTransform>();
            Vector3 _pos = _rect.anchoredPosition;
            Quaternion _rotation = _rect.rotation;
            int _order = m_cards[i].GetComponent<Canvas>().sortingOrder;
            m_cardSlots[i] = new CardSlot(_pos, _rotation, _order);

            //Instantiate Card Slot Image (Dot or whatever)
            if (m_cardSlotImage != null)
            {
                Image _new = Instantiate(m_cardSlotImage, Vector2.zero, Quaternion.identity, m_cardSlotHolder);
                _new.rectTransform.anchoredPosition = _pos + new Vector3(0, 0, -1);
                _new.rectTransform.rotation = _rotation;
            }
        }
        #endregion

        #region Initiate Bundle
        m_selectedCardIndex = 0;
        AddCard(m_startBundle);
        #endregion
    }

    private void Update()
    {
        if (state_paused) return;

        #region Input
        if (Input.GetButtonDown("Next Weapon") || (Input.GetAxis("Mouse ScrollWheel") < -0.1))
        {
            SwitchCard(-1);
        }

        if (Input.GetButtonDown("Previous Weapon") || (Input.GetAxis("Mouse ScrollWheel") > 0.1))
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

    private void OnDestroy()
    {
        NeonRounds.instance.gameData.GAME_ContinueLevel.RemoveListener(Unpause);
        NeonRounds.instance.gameData.GAME_PauseLevel.RemoveListener(Pause);
    }

    #region Card System Methods
    //Notes:
    //Card Deletion is handled in each Card object by setting itself inactive

    public bool AddCard(GameObject _newBundle)
    {
        #region Check Conditions
        //If there's only one card in the deck, check if this card is start bundle
        if (m_slotUsedCount == 1)
        {
            if (m_cards[m_selectedCardIndex].GetWeaponName() 
                == m_startBundle.GetComponentInChildren<WeaponParent>().gameObject.name)
            {
                m_cards[m_selectedCardIndex].DeleteCardAndBundle();
            }
        }

        //Check if slots are full. This step should be done before the method is called
        if (m_slotUsedCount >= m_cards.Length) return false;
        #endregion

        #region Adding Card
        m_slotUsedCount++;

        //Update Card Position to make room for new cards
        if (m_centerIfOnlyOneCard) UpdateCardPosition();

        //Instantiate gameobject
        GameObject _new = Instantiate(_newBundle, Vector2.zero, Quaternion.identity);

        //Create a new card at next available slot
        int _slotPosition = (m_slotUsedCount > 1) ? m_slotUsedCount - 1 : m_selectedCardIndex;
        m_cards[_slotPosition].CreateCard(_new);

        //Set it inactive if there are other cards
        if (_slotPosition != m_selectedCardIndex) _new.SetActive(false);
        else m_cards[_slotPosition].SelectCard(true);

        return true;
        #endregion
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
        if (m_slotUsedCount == 0) return;

        //If we want to center the card if there's only one card, Move the card to the middle
        #region Moving Card to Middle
        if (m_slotUsedCount == 1 && m_centerIfOnlyOneCard)
        {
            int _middle = m_cards.Length / 2;
            if (m_selectedCardIndex != _middle)
            {
                SwapCard(m_selectedCardIndex, _middle);
                m_selectedCardIndex = _middle;
            }
            return;
        }
        #endregion


        //If the card count >= 1
        for (int i = 1; i < m_cards.Length; i++)
        {
            //Update sort order
            if(i != m_selectedCardIndex) m_cards[i].UpdateSortOrder(m_cardSlots[i].st_sortingOrder);

            //Move cards if needed
            if (m_cards[i].isActive && !m_cards[i - 1].isActive)
            {
                //Change selected card index if applies
                if (m_selectedCardIndex == i) m_selectedCardIndex = i - 1;
                SwapCard(i, i - 1);
            }
        }
    }

    void SwapCard(int _this, int _swapTo)
    {
        //Get a copy of card in position i - 1
        Card _temp = m_cards[_swapTo];

        //Stop existing Dotween operations
        DOTween.Kill(m_cards[_this].GetComponent<RectTransform>());
        DOTween.Kill(m_cards[_swapTo].GetComponent<RectTransform>());

        //Move cards to new position (Swap position)
        RectTransform _rect;
        Canvas _canvas;
        //this card, uses dotween
        _rect = m_cards[_this].GetComponent<RectTransform>();
        _canvas = _rect.GetComponent<Canvas>();
        _rect.DOAnchorPos(m_cardSlots[_swapTo].st_slotPos, m_cardMoveDuration);
        _rect.DORotateQuaternion(m_cardSlots[_swapTo].st_slotRotation, m_cardMoveDuration);
        m_cards[_this].UpdateSortOrder(m_cardSlots[_swapTo].st_sortingOrder);
        //swap to card, immediate position change
        _rect = m_cards[_swapTo].GetComponent<RectTransform>();
        _canvas = _rect.GetComponent<Canvas>();
        _rect.anchoredPosition = m_cardSlots[_this].st_slotPos;
        _rect.rotation = m_cardSlots[_this].st_slotRotation;
        m_cards[_swapTo].UpdateSortOrder(m_cardSlots[_this].st_sortingOrder);

        //Update cards array
        m_cards[_swapTo] = m_cards[_this];
        m_cards[_this] = _temp;
    }
    #endregion
}
