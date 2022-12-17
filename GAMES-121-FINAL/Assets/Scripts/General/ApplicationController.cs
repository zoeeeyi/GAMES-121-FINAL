using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Application Controller", menuName = "Scriptable Objects/Application Controller")]
public class ApplicationController : ScriptableObject
{
    #region Events
    public UnityEvent APP_QuitGame;
    #endregion

    private void Start()
    {
        #region Subscribe Base Functions
        APP_QuitGame.AddListener(QuitGame);
        #endregion
    }

    #region Base Functions
    void QuitGame()
    {
        APP_QuitGame.RemoveAllListeners();
        Application.Quit();
    }
    #endregion
}
