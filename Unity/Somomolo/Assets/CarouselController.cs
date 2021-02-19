using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselController : MonoBehaviour
{
    public static CarouselController instance;
    [SerializeField] public Transform limitRight;
    [SerializeField] public Transform limitLeft;

    void Awake()
    {
        instance = this;
    }
}
    
