using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameStateMachine
{
    public enum InputType
    {
        Key,
        MenuButton
    }
    public abstract void HandleRestartLevel(InputType _inputType);
    public abstract void HandleExit(InputType _inputType);
}

public class InGame : GameStateMachine
{
    public override void HandleRestartLevel(InputType _inputType)
    {
        Time.timeScale = 1;
        GameController.instance.GAME_RestartLevel.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void HandleExit(InputType _inputType)
    {
        //Load Pause Menu
    }
}

public class InPauseMenu : GameStateMachine
{
    public override void HandleRestartLevel(InputType _inputType) 
    {
        switch (_inputType)
        {
            case InputType.Key:
                return;
                
            case InputType.MenuButton:
                Time.timeScale = 1;
                GameController.instance.GAME_RestartLevel.Invoke();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }

    public override void HandleExit(InputType _inputType)
    {
        //Back to InGame state
    }
}

public class InMainMenu : GameStateMachine
{
    public override void HandleRestartLevel(InputType _inputType)
    {
        //To be implemented
        return;
    }

    public override void HandleExit(InputType _inputType)
    {
        //To be implemented
        return;
    }
}
