using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterController : MonoBehaviour
{
    [SerializeField] public GameObject slot;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PopOut()
    {
        animator.SetTrigger("popOut");
    }

    public void PopIn()
    {
        animator.SetTrigger("popIn");
    }
}
