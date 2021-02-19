using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselHorseController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        animator.Play("HorseMoving", 0, Random.Range(0f, 1f));
    }

    void Update()
    {
        
    }
}
