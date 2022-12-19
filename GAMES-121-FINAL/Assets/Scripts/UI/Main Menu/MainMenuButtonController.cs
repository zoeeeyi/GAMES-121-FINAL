using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] LevelController m_levelController;
    [SerializeField] GameObject[] m_buttons;
    int m_currentButtonIndex = 0;

    #region FOR DEVELOPMENT!!!
    public void SpeedRunButton(string _levelName)
    {
        m_levelController.LoadLevel(_levelName);
    }
    #endregion

    #region Main Buttons
    public void ExitButton()
    {
        GameController.instance.GameStateMachine.HandleExit(GameStateMachine.InputType.MenuButton);
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
