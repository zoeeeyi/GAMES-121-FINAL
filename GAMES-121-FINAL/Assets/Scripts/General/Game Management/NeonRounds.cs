using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Controller Pipeline:
/// 1. Set game state and load respective state machine
/// 2. Allow other game objects to subscribe to game events, if applies
/// 3. Execute initialize game, and let the state machine decide what to do
/// </summary>

public class NeonRounds : MonoBehaviour
{
    public static NeonRounds instance;
    public GameData gameData;
    [SerializeField] bool m_devMode = false;
    [SerializeField] bool m_deleteSaveData = false;
    

    #region Game Modes
    [SerializeField] GameMode m_awakeGameMode;
    public enum GameMode
    {
        None,
        Speedrun,
        Freerun,
        Katana
    }
    #endregion

    #region State Machine
    [SerializeField] GameState m_awakeState;
    public GameState currentGameState { get; private set; }
    public enum GameState
    {
        InLevelPrep,
        InGame,
        InPauseMenu,
        InTransitionMenu,
        InLoseMenu,
        InMainMenu,
    }

    public GameStateMachine GameStateMachine { get; private set; }
    GameStateMachine GSM_inMainMenu = new InMainMenu();
    GameStateMachine GSM_inLevelPrep = new InLevelPrep();
    GameStateMachine GSM_inGame = new InGame();
    GameStateMachine GSM_inTransitionMenu = new InTransitionMenu();
    GameStateMachine GSM_inLoseMenu = new InLoseMenu();
    GameStateMachine GSM_inPauseMenu = new InPauseMenu();

    public void ChangeGameState(GameState _state)
    {
        currentGameState = _state;
        switch (_state)
        {
            case GameState.InLevelPrep:
                GameStateMachine = GSM_inLevelPrep;
                break;

            case GameState.InGame:
                GameStateMachine = GSM_inGame;
                break;

            case GameState.InPauseMenu:
                GameStateMachine = GSM_inPauseMenu;
                break;

            case GameState.InTransitionMenu:
                GameStateMachine = GSM_inTransitionMenu;
                break;

            case GameState.InLoseMenu:
                GameStateMachine = GSM_inLoseMenu;
                break;

            case GameState.InMainMenu:
                GameStateMachine = GSM_inMainMenu;
                break;
        }
        GameStateMachine.HandleInitializeState();
    }
    #endregion

    #region Mono Behavior
    private void Awake()
    {
        #region Set up game manager as dont destroy on load
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //Set game state and mode
            gameData.SetGameModeAndLevel(m_awakeGameMode, SceneManager.GetActiveScene().name);
            if (m_devMode) gameData.SaveGameData(true);
            if (m_deleteSaveData) gameData.DeleteGameData();
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        #region Event Subscription
        gameData.GAME_FailLevel.AddListener(FailLevel);
        gameData.GAME_QuitGame.AddListener(QuitGame);
        #endregion
        ChangeGameState(m_awakeState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) Time.timeScale = 1;

        #region Handle Input
        if (Input.GetButtonDown("Restart Level"))
        {
            GameStateMachine.HandleRestartLevel(GameStateMachine.TriggerType.Key);
        }

        if (Input.GetButtonDown("Exit"))
        {
            GameStateMachine.HandleExit(GameStateMachine.TriggerType.Key);
        }
        #endregion
    }
    #endregion

    #region Utility Methods
    //Delayed Invoke would come handy when something is invoked immediately after a scene reload
    public void DelayedInvoke(UnityEvent _event, float _delay = 0.1f) 
    {
        StartCoroutine(DelayedInvokeRoutine(_event, _delay));
    }

    IEnumerator DelayedInvokeRoutine(UnityEvent _event, float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        _event.Invoke();
    }
    #endregion

    #region Game Event Methods

    #region Game Save, load and quit
    public void LoadLevel(string _targetLevel = null, GameState _targetGameState = GameState.InLevelPrep)
    {
        if (_targetLevel == null) _targetLevel = gameData.currentLevel;
        SceneManager.LoadScene(_targetLevel);
        ChangeGameState(_targetGameState);
    }

    public void StartNewGame(GameMode _gameMode, string _level = null)
    {
        if (_level == null) _level = gameData.levelList[0];
        gameData.SetGameModeAndLevel(_gameMode, _level);
        gameData.SaveGameData(true);
        LoadLevel();
    }

    public void ContinueSavedGame()
    {
        gameData.LoadGameData();
        LoadLevel();
    }

    void QuitGame()
    {
        gameData.GAME_StartLevelPrep.RemoveAllListeners();
        gameData.GAME_QuitGame.RemoveAllListeners();
        Application.Quit();
    }
    #endregion

    #region Game Win and Lose
    public void WinLevel(string _nextLevelName)
    {
        gameData.GAME_WinLevel.Invoke();
        switch (gameData.currentGameMode)
        {
            case GameMode.Speedrun:
                gameData.SetRemainingTime(Clock.instance.currentTime);
                gameData.TrySetSpeedRunBestTime(gameData.currentSessionRemainingTime, gameData.currentLevel);
                break;

            case GameMode.Freerun:
                float _thisRunTime = Clock.instance.currentTime - gameData.currentSessionElapsedTime;
                gameData.SetElapsedTime(Clock.instance.currentTime);
                gameData.TrySetFreeRunBestTime(_thisRunTime, gameData.currentLevel);
                break;
        }
        gameData.SetGemCollected(gameData.gemCollected + 1);
        gameData.SetGameModeAndLevel(gameData.currentGameMode, _nextLevelName);
        gameData.SaveGameData();
    }

    //Fail event is invoked by the clock
    public void FailLevel()
    {
        ChangeGameState(GameState.InLoseMenu);
    }
    #endregion

    #endregion
}
