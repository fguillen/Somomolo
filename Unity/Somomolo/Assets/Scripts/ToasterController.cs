using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterController : MonoBehaviour
{
    [SerializeField] public GameObject slot;    
    [SerializeField] AudioClip[] clipsPopIn;
    [SerializeField] AudioClip[] clipsPopOut;
    public ToastController toast;


    string state;

    Animator animator;
    AudioSource audioSource;
    void Awake()
    {
        state = "idle";
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PopOut()
    {
        animator.SetTrigger("popOut");
        state = "popingOut";
        PlayPopOut();
    }

    public void PopIn()
    {
        animator.SetTrigger("popIn");
        state = "popingIn";
        PlayPopIn();
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

    void PlayPopIn()
    {
        var clip = clipsPopIn[Random.Range(0, clipsPopIn.Length)];
        audioSource.PlayOneShot(clip);
    }

    void PlayPopOut()
    {
        var clip = clipsPopOut[Random.Range(0, clipsPopOut.Length)];
        audioSource.PlayOneShot(clip);
    }


}
