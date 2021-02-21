using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] public GameObject slot;
    [SerializeField] AudioClip[] clipsPopIn;
    [SerializeField] AudioClip[] clipsPopOut;
    public PickController pick;


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

    void PickClicked()
    {
        animator.SetTrigger("clicked");
        state = "clicked";
    }

    void AnimationFinished()
    {
        if(state == "clicked")
        {
            SpawnersController.instance.ClickedPick(pick); 
        }

        state = "idle";
    }

    void OnMouseDown()
    {
        if(
            SpawnersController.instance.IsNotFinished() &&
            SpawnersController.instance.AreAllSpawnersIdle()
        )
        {
            PickClicked();
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
