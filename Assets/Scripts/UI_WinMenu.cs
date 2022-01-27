using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_WinMenu : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene("TestGrounds");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
