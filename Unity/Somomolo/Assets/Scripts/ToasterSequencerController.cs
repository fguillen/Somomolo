using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ToasterSequencerController : MonoBehaviour
{
    [SerializeField] ToastController[] toasts;
    [SerializeField] ToasterController[] toasters;
    List<ToastController[]> sequences;

    void Awake()
    {
        sequences = new List<ToastController[]>();    
    }

    void Start()
    {
        SpawnNewSequence();
    }

    public ToastController[] CreateNewSequence()
    {
        ToastController[] sequence = new ArraySegment<ToastController>(toasts, sequences.Count, 3).Array;
        sequences.Add(sequence);

        return sequence;
    }

    public void SpawnNewSequence()
    {
        var sequence = CreateNewSequence();

        for (int i = 0; i < toasters.Length; i++)
        {
            var toastPrefab = sequence[i].gameObject;
            var toaster = toasters[i];

            // GameObject toast = Instantiate(toastPrefab, Vector3.zero, Quaternion.identity, toaster.slot.transform, false);
            GameObject toast = Instantiate(toastPrefab, toaster.slot.transform);
            // toast.transform.parent = toaster.slot.transform;
        }
    }

}
