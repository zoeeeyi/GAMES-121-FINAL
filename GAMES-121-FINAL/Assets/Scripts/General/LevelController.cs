using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level Controller", menuName = "Scriptable Objects/Level Controller")]
public class LevelController : ScriptableObject
{
    public void LoadLevel(string _levelName)
    {
        SceneManager.LoadScene(_levelName);
    }
}
