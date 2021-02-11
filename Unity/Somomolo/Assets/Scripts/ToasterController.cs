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
        if(state == "idle")
        {
            animator.SetTrigger("popOut");
            state = "popingOut";
        }
    }

    public void PopIn()
    {
        if(state == "idle")
        {
            animator.SetTrigger("popIn");
            state = "popingIn";
        }
    }

    public void ToastClicked()
    {
        if(state == "idle")
        {
            animator.SetTrigger("clicked");
            state = "clicked";
        }
    }

    public void AnimationFinished()
    {
        if(state == "clicked")
        {
            state = "idle"; // It has to be idle before we start the chain sequence
            ToasterSequencerController.instance.ClickedToast(toast); 
        }

        state = "idle";
    }
}
