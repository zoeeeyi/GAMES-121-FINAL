using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.XR;

public class Clock : MonoBehaviour
{
    public static Clock instance;
    [SerializeField] GameData m_gameData;

    //New
    #region Clock Mode Variables
    ClockMode m_clockMode;
    public enum ClockMode
    {
        StopWatch,
        Timer
    }
    #endregion

    #region Clock Value Variables
    [Range(0, float.MaxValue)]
    float m_startTime;
    public float currentTime { get; private set; }
    #endregion

    public bool state_paused {get; private set; }

    #region Mono Behavior
    private void Awake()
    {
        #region Set up game manager as dont destroy on load
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            //Events Subscription
            m_gameData.GAME_StartLevelPrep.AddListener(ResetClock);
            m_gameData.GAME_PauseLevel.AddListener(PauseClock);
            m_gameData.GAME_ContinueLevel.AddListener(UnpauseClock);
            m_gameData.GAME_WinLevel.AddListener(PauseClock);
            m_gameData.GAME_FailLevel.AddListener(PauseClock);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        state_paused = true;
    }

    void Update()
    {
        if (state_paused) return;
        switch (m_clockMode)
        {
            case ClockMode.StopWatch:
                RunStopWatch();
                break;

            case ClockMode.Timer:
                RunTimer();
                break;
        }
    }

    private void OnDestroy()
    {
        m_gameData.GAME_StartLevelPrep.RemoveListener(ResetClock);
        m_gameData.GAME_PauseLevel.RemoveListener(PauseClock);
        m_gameData.GAME_ContinueLevel.RemoveListener(UnpauseClock);
        m_gameData.GAME_WinLevel.RemoveListener(PauseClock);
        m_gameData.GAME_FailLevel.RemoveListener(PauseClock);
    }
    #endregion

    #region Utility Methods
    public string GetCurrentTimeString()
    {
        return TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
    }
    #endregion

    #region Clock Mode Methods
    void RunStopWatch()
    {
        currentTime += Time.deltaTime;
    }

    void RunTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0) m_gameData.GAME_FailLevel.Invoke();
    }
    #endregion

    #region Clock Main Methods
    public void SetClock(float _newTime, ClockMode _mode)
    {
        SetClock(_newTime);
        SetClock(_mode);
    }

    public void SetClock(float _newTime, bool _delta = false)
    {
        if (_delta) currentTime += _newTime;
        else currentTime = _newTime;
    }

    public void SetClock(ClockMode _mode)
    {
        m_clockMode = _mode;
    }

    public void UnpauseClock()
    {
        state_paused = false;
    }

    public void PauseClock()
    {
        state_paused = true;
    }
    #endregion

    #region Game Related Methods
    void ResetClock()
    {
        //Set Clock
        switch (NeonRounds.instance.gameData.currentGameMode)
        {
            case NeonRounds.GameMode.Speedrun:
                SetClock(NeonRounds.instance.gameData.currentSessionRemainingTime, Clock.ClockMode.Timer);
                break;

            case NeonRounds.GameMode.Freerun:
                SetClock(NeonRounds.instance.gameData.currentSessionElapsedTime, Clock.ClockMode.StopWatch);
                break;
        }
    }
    #endregion
}