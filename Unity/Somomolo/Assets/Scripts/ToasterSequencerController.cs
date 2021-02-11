using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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

    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            StartCoroutine("PopInToasts");
        }
    }

    void RemoveOldToasts()
    {
        foreach (var toasterController in toasters)
        {
            foreach (Transform child in toasterController.slot.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    ToastController[] CreateNewSequence()
    {
        ToastController[] sequence = new ArraySegment<ToastController>(toasts, sequences.Count, 3).ToArray();
        sequences.Add(sequence);

        return sequence;
    }

    ToasterController[] ShuffledToasters()
    {
        var random = new System.Random();
        return toasters.OrderBy(i => random.Next()).ToArray();
    }

    void SpawnNewSequence()
    {
        var sequence = CreateNewSequence();
        var shuffledToasters = ShuffledToasters();

        for (int i = 0; i < shuffledToasters.Length; i++)
        {
            var toastPrefab = sequence[i].gameObject;
            var toaster = shuffledToasters[i];

            Instantiate(toastPrefab, toaster.slot.transform);
        }

        StartCoroutine("PopOutToasts");
    }

    IEnumerator PopOutToasts()
    {
        var shuffledToasters = ShuffledToasters();
        
        foreach (var toasterController in shuffledToasters)
        {
            toasterController.PopOut();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
        }
    }

    IEnumerator PopInToasts()
    {
        var shuffledToasters = ShuffledToasters();
        
        foreach (var toasterController in shuffledToasters)
        {
            toasterController.PopIn();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
        }

        RemoveOldToasts();
        SpawnNewSequence();
    }

}
