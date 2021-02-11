using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGameController : MonoBehaviour
{
    public static CanvasGameController instance;
    Animator animator;

    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public void ShowWrong()
    {
        animator.SetTrigger("showWrong");
    }

    public void ShowCorrect()
    {
        animator.SetTrigger("showCorrect");
    }
    
    public void ShowYouWin()
    {
        animator.SetTrigger("showYouWin");
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
}
