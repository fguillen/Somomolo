using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController instance;
    [SerializeField] public Transform[] jumpingPoints;

    void Awake()
    {
        instance = this;
    }
}
