using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionSceneUI : MonoBehaviour
{
    bool m_imEnding = false;
    [SerializeField] int m_autoToNextLevelTime = 4;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI m_clockModeText;
    [SerializeField] TextMeshProUGUI m_currentTimeText;
    [SerializeField] TextMeshProUGUI m_bestRecordText;
    [SerializeField] TextMeshProUGUI m_bestRecordTimeText;

    [Header("Animations")]
    [SerializeField] Animator m_gems;
    [SerializeField] Animator m_credits;

    private void Start()
    {
        //Check if this is the ending
        int _thisLevelIndex = NeonRounds.instance.gameData.levelDic[NeonRounds.instance.gameData.currentLevel];
        if (NeonRounds.instance.gameData.gemCollected == NeonRounds.instance.gameData.totalGems) m_imEnding = true;
        else _thisLevelIndex--;

        //Play gem animation
        switch (NeonRounds.instance.gameData.gemCollected)
        {
            case 1:
                m_gems.SetTrigger("1 Gem");
                break;

            case 2:
                m_gems.SetTrigger("2 Gems");
                break;

            case 3:
                m_gems.SetTrigger("3 Gems");
                break;
        }

        //Get records
        string _thisLevelName = NeonRounds.instance.gameData.levelList[_thisLevelIndex];
        float _thisLevelRecord = new float();
        float _currentTime = new float();

        switch (NeonRounds.instance?.gameData.currentGameMode)
        {
            case NeonRounds.GameMode.Speedrun:
                m_clockModeText.text = "Remaining Time:";
                _currentTime = NeonRounds.instance.gameData.currentSessionRemainingTime;
                if (!m_imEnding) _thisLevelRecord = NeonRounds.instance.gameData.speedRunBestTime[_thisLevelName];
                else _thisLevelRecord = NeonRounds.instance.gameData.speedRunBestTime["Whole Game"];
                break;

            case NeonRounds.GameMode.Freerun:
                m_clockModeText.text = "Elapsed Time:";
                _currentTime = NeonRounds.instance.gameData.currentSessionElapsedTime;
                bool _hasValue = NeonRounds.instance.gameData.freerunBestTime.TryGetValue("Whole Game", out float _time);
                if (!_hasValue) _thisLevelRecord = -100;
                else _thisLevelRecord = _time;
                break;

            default:
                m_clockModeText.gameObject.SetActive(false);
                m_currentTimeText.gameObject.SetActive(false);
                m_bestRecordText.gameObject.SetActive(false);
                m_bestRecordText.gameObject.SetActive(false);
                break;
        }

        m_currentTimeText.text = TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss");
        if (_thisLevelRecord != -100)
        {
            m_bestRecordTimeText.text = TimeSpan.FromSeconds(_thisLevelRecord).ToString(@"mm\:ss");
        } else
        {
            m_bestRecordTimeText.text = "No Record!";
        }

        if (!m_imEnding) StartCoroutine(AutoSwitchScene());
        else StartCoroutine(PlayCredits());
    }

    IEnumerator AutoSwitchScene()
    {
        yield return new WaitForSecondsRealtime(m_autoToNextLevelTime);
        NeonRounds.instance?.LoadLevel(NeonRounds.instance.gameData.currentLevel, NeonRounds.GameState.InLevelPrep);
    }

    IEnumerator PlayCredits()
    {
        yield return new WaitForSecondsRealtime(m_autoToNextLevelTime);
        m_credits.SetTrigger("Play");
    }
}
