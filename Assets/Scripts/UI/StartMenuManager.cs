using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BeginIntro()
    {
        SceneManager.LoadScene("IntroCutScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Controls()
    {
        animator.SetBool("Controls", true);
    }

    public void Credits()
    {
        animator.SetBool("Credits", true);
    }

    public void Back()
    {
        animator.SetBool("Credits", false);
        animator.SetBool("Controls", false);
    }
}
