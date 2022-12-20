using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] GameData m_levelController;
    [SerializeField] GameObject[] m_buttons;
    int m_currentButtonIndex = 0;

    //Temp, may find better solution
    [BoxGroup("Continue Button")] [SerializeField] TextMeshProUGUI m_continueButtonText;
    [BoxGroup("Continue Button")] [SerializeField] Image m_continueButtonImage;
    [BoxGroup("Continue Button")] [SerializeField] Button m_continueButton;

    private void Start()
    {
        if (m_levelController.LoadGameData(true))
        {
            #region Set Continue Button active
            m_continueButton.interactable = true;
            Color _newColor = m_continueButtonImage.color;
            _newColor.a = 1;
            m_continueButtonImage.color = _newColor;
            _newColor = m_continueButtonText.color;
            _newColor.a = 1;
            m_continueButtonText.color = _newColor;
            #endregion
        }
    }

    #region Main Buttons
    public void ContinueButton()
    {
        NeonRounds.instance.ContinueSavedGame();
    }

    public void NewSpeedRunButton(string _levelName)
    {
        NeonRounds.instance.StartNewGame(NeonRounds.GameMode.Speedrun, _levelName);
    }

    public void NewFreerunButton(string _levelName)
    {
        NeonRounds.instance.StartNewGame(NeonRounds.GameMode.Freerun, _levelName);
    }

    public void ExitButton()
    {
        NeonRounds.instance.GameStateMachine.HandleExit(GameStateMachine.TriggerType.MenuButton);
    }
    #endregion

    #region Navigation Buttons
    public void RightButton()
    {
        int _nextIndex = (m_currentButtonIndex - 1 >= 0) ? m_currentButtonIndex - 1 : m_buttons.Length - 1;
        if (!m_buttons[_nextIndex].gameObject.activeInHierarchy) m_buttons[_nextIndex].gameObject.SetActive(true);
        m_buttons[_nextIndex].GetComponent<Animator>().SetTrigger("Left In");
        m_buttons[m_currentButtonIndex].GetComponent<Animator>().SetTrigger("Right Out");
        m_currentButtonIndex = _nextIndex;
    }

    public void LeftButton()
    {
        int _nextIndex = (m_currentButtonIndex + 1 < m_buttons.Length) ? m_currentButtonIndex + 1 : 0;
        if (!m_buttons[_nextIndex].gameObject.activeInHierarchy) m_buttons[_nextIndex].gameObject.SetActive(true);
        m_buttons[_nextIndex].GetComponent<Animator>().SetTrigger("Right In");
        m_buttons[m_currentButtonIndex].GetComponent<Animator>().SetTrigger("Left Out");
        m_currentButtonIndex = _nextIndex;
    }
    #endregion
}
