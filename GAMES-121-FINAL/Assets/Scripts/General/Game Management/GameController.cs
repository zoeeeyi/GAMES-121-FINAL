using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    #region Events
    //These events should do the same thing when called, regardless of current game state
    [HideInInspector] public UnityEvent GAME_StartLevel;
    [HideInInspector] public UnityEvent GAME_RestartLevel;
    [HideInInspector] public UnityEvent GAME_WinLevel;
    [HideInInspector] public UnityEvent GAME_FailLevel;
    [HideInInspector] public UnityEvent GAME_QuitGame;
    #endregion

    #region Game Modes
    public enum GameModes
    {
        Speedrun,
        Casual,
        Katana
    }
    #endregion

    #region State Machine
    [SerializeField] GameState m_awakeState;
    public GameState currentGameState { get; private set; }
    public enum GameState
    {
        InGame,
        InPauseMenu,
        InTransitionMenu,
        InMainMenu,
    }

    public GameStateMachine GameStateMachine { get; private set; }
    GameStateMachine GSM_inMainMenu = new InMainMenu();
    GameStateMachine GSM_inGame = new InGame();
    GameStateMachine GSM_inPauseMenu = new InPauseMenu();

    public void ChangeGameState(GameState _state)
    {
        currentGameState = _state;
        switch (_state)
        {
            case GameState.InGame:
                GameStateMachine = GSM_inGame;
                break;

            case GameState.InPauseMenu:
                GameStateMachine = GSM_inPauseMenu;
                break;

            case GameState.InMainMenu:
                GameStateMachine = GSM_inMainMenu;
                break;
        }
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
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        //Set game state
        ChangeGameState(m_awakeState);
    }

    private void Start()
    {
        #region Subscribe Base Functions
        GAME_QuitGame.AddListener(QuitGame);
        #endregion
    }

    private void Update()
    {
        #region Handle Input
        if (Input.GetButtonDown("Restart Level"))
        {
            GameStateMachine.HandleRestartLevel(GameStateMachine.InputType.Key);
        }

        if (Input.GetButtonDown("Exit"))
        {
            GameStateMachine.HandleExit(GameStateMachine.InputType.Key);
        }
        #endregion

    }
    #endregion

    #region Universal Functions
    void QuitGame()
    {
        GAME_QuitGame.RemoveAllListeners();
        Application.Quit();
    }
    #endregion
}
