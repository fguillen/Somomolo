using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ToasterSequencerController : MonoBehaviour
{
    public static ToasterSequencerController instance;
    [SerializeField] ToastController[] toasts;
    [SerializeField] ToasterController[] toasters;
    List<ToastController[]> sequences;

    string state = "starting";

    void Awake()
    {
        instance = this;
        sequences = new List<ToastController[]>();    
    }

    void Start()
    {
        state = "started";
        SpawnNewSequence();
    }

    bool AreThereMoreSequences()
    {
        return (toasts.Length > (sequences.Count() + toasters.Length - 1));
    }

    void WrongClick()
    {
        print("Wrong");
        // CanvasGameController.instance.ShowWrong();
        StartFromScratch();
        ChickensController.instance.SendChickensOutOfScene();
    }

    void CorrectClick()
    {
        print("Yes!");
        // CanvasGameController.instance.ShowCorrect();
        NextWave();
        ChickensController.instance.SendChickenToScene();
    }

    void StartFromScratch()
    {
        sequences.Clear();
        NextWave();
    }


    void NextWave()
    {
        if(AreThereMoreSequences())
        {
            StartCoroutine("PopInToasts");
        } else 
        {
            Win();
        }
    }

    void Win()
    {
        state = "finished";
        CanvasGameController.instance.ShowYouWin();
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

            var toast = Instantiate(toastPrefab, toaster.slot.transform).GetComponent<ToastController>();
            toaster.toast = toast;
        }

        StartCoroutine("PopOutToasts");
    }

    IEnumerator PopOutToasts()
    {
        var shuffledToasters = ShuffledToasters();
        
        foreach (var toasterController in shuffledToasters)
        {
            toasterController.PopOut();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
        }
    }

    IEnumerator PopInToasts()
    {
        var shuffledToasters = ShuffledToasters();
        
        foreach (var toasterController in shuffledToasters)
        {
            toasterController.PopIn();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
        }

        yield return new WaitForSeconds(0.2f);

        RemoveOldToasts();
        SpawnNewSequence();
    }

    public void ClickedToast(ToastController toastController)
    {
        if(IsTheCorrectToast(toastController))
        {
            CorrectClick();
        } else
        {
            WrongClick();
        }
    }

    bool IsTheCorrectToast(ToastController toast)
    {
        var lastSequence = sequences.Last();
        var previousSequence = Array.Empty<ToastController>();

        if(sequences.Count() > 1)
        {
            previousSequence = sequences[sequences.Count() - 2];
        }

        var newToasts = lastSequence.Except(previousSequence).ToArray();

        return Array.Exists(newToasts, e => e.toastName == toast.toastName);
    }

    public bool AreAllToastersIdle()
    {
        return toasters.All(e => e.IsIdle());
    }

    public bool IsNotFinished()
    {
        return state != "finished";
    }
}
