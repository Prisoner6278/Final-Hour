using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void BeginGame()
    {
        SceneManager.LoadScene("Day1Scene");
    }

    public void BeginIntro()
    {
        SceneManager.LoadScene("IntroCutScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
