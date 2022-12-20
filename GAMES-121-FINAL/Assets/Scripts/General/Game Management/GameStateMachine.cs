using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class GameStateMachine
{
    public enum TriggerType
    {
        Key,
        MenuButton
    }
    public abstract void HandleInitializeState();
    public abstract void HandleRestartLevel(TriggerType _inputType);
    public abstract void HandleExit(TriggerType _inputType);

    #region Commonly used/Universal Methods
    protected virtual void ExecuteContinueLevel()
    {
        Time.timeScale = 1;
        NeonRounds.instance.ChangeGameState(NeonRounds.GameState.InGame);
        NeonRounds.instance.gameData.GAME_ContinueLevel.Invoke();
    }

    protected virtual void ExecuteRestartLevel()
    {
        Time.timeScale = 0;
        NeonRounds.instance.gameData.GAME_RestartLevel.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        NeonRounds.instance.ChangeGameState(NeonRounds.GameState.InLevelPrep);
    }

    protected virtual void ExecutePauseLevel()
    {
        ExecutePauseLevel(false);
    }

    protected virtual void ExecutePauseLevel(bool _delayedInvoke, float _delay = 0)
    {
        Time.timeScale = 0;
        if (_delayedInvoke) NeonRounds.instance.DelayedInvoke(NeonRounds.instance.gameData.GAME_PauseLevel, _delay);
        else NeonRounds.instance.gameData.GAME_PauseLevel.Invoke();
    }

    protected virtual void ExecuteQuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(NeonRounds.instance.gameData.mainMenuName);
        NeonRounds.instance.ChangeGameState(NeonRounds.GameState.InMainMenu);
    }
    #endregion
}

public class InLevelPrep : GameStateMachine
{
    public override void HandleInitializeState() { ExecuteLevelPrep(); }

    public override void HandleExit(TriggerType _inputType) { return; }

    public override void HandleRestartLevel(TriggerType _inputType) { ExecuteContinueLevel(); }

    void ExecuteLevelPrep()
    {
        Time.timeScale = 0;
        //Level is reloaded before in level prep. we therefore need a delayed invoke
        ExecutePauseLevel(true);
        NeonRounds.instance.DelayedInvoke(NeonRounds.instance.gameData.GAME_StartLevelPrep);
    }
}

public class InGame : GameStateMachine
{
    public override void HandleInitializeState()
    {
        return;
    }

    public override void HandleRestartLevel(TriggerType _inputType)
    {
        ExecuteRestartLevel();
    }

    public override void HandleExit(TriggerType _inputType)
    {
        ExecutePauseLevel();
    }

    #region Method Overrides
    protected override void ExecutePauseLevel()
    {
        NeonRounds.instance.ChangeGameState(NeonRounds.GameState.InPauseMenu);
    }
    #endregion
}

public class InPauseMenu : GameStateMachine
{
    public override void HandleInitializeState()
    {
        ExecutePauseLevel();
    }

    public override void HandleRestartLevel(TriggerType _inputType) 
    {
        switch (_inputType)
        {
            case TriggerType.Key:
                break;
                
            case TriggerType.MenuButton:
                ExecuteRestartLevel();
                break;
        }
    }

    public override void HandleExit(TriggerType _inputType)
    {
        switch (_inputType)
        {
            case TriggerType.Key:
                ExecuteContinueLevel();
                break;

            case TriggerType.MenuButton:
                ExecuteQuitToMainMenu();
                break;
        }
    }
}

public class InLoseMenu : GameStateMachine
{
    public override void HandleInitializeState()
    {
        ExecutePauseLevel();
    }

    public override void HandleExit(TriggerType _inputType)
    {
        switch (_inputType)
        {
            case TriggerType.Key:
                return;

            case TriggerType.MenuButton:
                ExecuteQuitToMainMenu();
                break;
        }
    }

    public override void HandleRestartLevel(TriggerType _inputType)
    {
        switch (_inputType)
        {
            case TriggerType.Key:
                return;

            case TriggerType.MenuButton:
                ExecuteRestartLevel();
                break;
        }
    }
}

public class InTransitionMenu : GameStateMachine
{
    public override void HandleExit(TriggerType _inputType)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleInitializeState()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleRestartLevel(TriggerType _inputType)
    {
        throw new System.NotImplementedException();
    }
}

public class InMainMenu : GameStateMachine
{
    public override void HandleInitializeState()
    {
        //To be implemented
        return;
    }

    public override void HandleRestartLevel(TriggerType _inputType) { return; }

    public override void HandleExit(TriggerType _inputType)
    {
        //To be implemented
        return;
    }
}
