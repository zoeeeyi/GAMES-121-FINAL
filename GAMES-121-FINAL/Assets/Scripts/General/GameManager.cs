using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        #region Input
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion

        #region Test (For development)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endregion
    }
}
