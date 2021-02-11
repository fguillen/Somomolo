using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterController : MonoBehaviour
{
    [SerializeField] public GameObject slot;
    public ToastController toast;

    string state;

    Animator animator;

    void Awake()
    {
        state = "idle";
        animator = GetComponent<Animator>();
    }

    public void PopOut()
    {
        animator.SetTrigger("popOut");
        state = "popingOut";
    }

    public void PopIn()
    {
        animator.SetTrigger("popIn");
        state = "popingIn";
    }

    void ToastClicked()
    {
        animator.SetTrigger("clicked");
        state = "clicked";
    }

    void AnimationFinished()
    {
        if(state == "clicked")
        {
            ToasterSequencerController.instance.ClickedToast(toast); 
        }

        state = "idle";
    }

    void OnMouseDown()
    {
        if(
            ToasterSequencerController.instance.IsNotFinished() &&
            ToasterSequencerController.instance.AreAllToastersIdle()
        )
        {
            ToastClicked();
        }
    }

    public bool IsIdle()
    {
        return state == "idle";
    }
}
