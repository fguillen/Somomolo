using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class SpawnersController : MonoBehaviour
{
    public static SpawnersController instance;
    [SerializeField] PickController[] picks;
    [SerializeField] SpawnerController[] spawners;

    [SerializeField] AudioClip clipCorrect;
    [SerializeField] AudioClip clipNoCorrect;
    [SerializeField] AudioClip clipWin;

    List<PickController[]> sequences;

    string state = "starting";

    AudioSource audioSource;

    void Awake()
    {
        instance = this;
        sequences = new List<PickController[]>();
        audioSource = GetComponent<AudioSource>();    
    }

    void Start()
    {
        state = "started";
        SpawnNewSequence();
    }

    bool AreThereMoreSequences()
    {
        return (picks.Length > (sequences.Count() + spawners.Length - 1));
    }

    void WrongClick()
    {
        print("Wrong");
        StartFromScratch();
        // ChickensController.instance.SendChickensOutOfScene();
        ConfettiController.instance.SweepConfetti();
        audioSource.PlayOneShot(clipNoCorrect);
    }

    void CorrectClick()
    {
        print("Yes!");
        NextWave();
        // ChickensController.instance.SendChickenToScene();
        audioSource.PlayOneShot(clipCorrect);
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
            StartCoroutine("PopInPicks");
        } else 
        {
            Win();
        }
    }

    void Win()
    {
        state = "finished";
        CanvasGameController.instance.ShowYouWin();
        audioSource.PlayOneShot(clipWin);
    }

    void RemoveOldPicks()
    {
        foreach (var spawnerController in spawners)
        {
            foreach (Transform child in spawnerController.slot.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    PickController[] CreateNewSequence()
    {
        PickController[] sequence = new ArraySegment<PickController>(picks, sequences.Count, spawners.Length).ToArray();
        sequences.Add(sequence);

        return sequence;
    }

    SpawnerController[] ShuffledSpawners()
    {
        var random = new System.Random();
        return spawners.OrderBy(i => random.Next()).ToArray();
    }

    void SpawnNewSequence()
    {
        var sequence = CreateNewSequence();
        var shuffledSpawners = ShuffledSpawners();

        for (int i = 0; i < shuffledSpawners.Length; i++)
        {
            var pickPrefab = sequence[i].gameObject;
            var spawner = shuffledSpawners[i];

            var pick = Instantiate(pickPrefab, spawner.slot.transform).GetComponent<PickController>();
            spawner.pick = pick;
        }

        StartCoroutine("PopOutToasts");
    }

    IEnumerator PopOutToasts()
    {
        var shuffledSpawners = ShuffledSpawners();
        
        foreach (var spawnerController in shuffledSpawners)
        {
            spawnerController.PopOut();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
        }
    }

    IEnumerator PopInPicks()
    {
        var shuffledSpawners = ShuffledSpawners();
        
        foreach (var spawnerController in shuffledSpawners)
        {
            spawnerController.PopIn();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
        }

        yield return new WaitForSeconds(0.2f);

        RemoveOldPicks();
        SpawnNewSequence();
    }

    public void ClickedPick(PickController pickController)
    {
        if(IsTheCorrectPick(pickController))
        {
            CorrectClick();
        } else
        {
            WrongClick();
        }
    }

    bool IsTheCorrectPick(PickController pick)
    {
        print("IsTheCorrentPick");

        var lastSequence = sequences.Last();
        var previousSequence = Array.Empty<PickController>();

        if(sequences.Count() > 1)
        {
            previousSequence = sequences[sequences.Count() - 2];
        }

        var newPicks = lastSequence.Except(previousSequence).ToArray();
        var result = Array.Exists(newPicks, e => e.pickName == pick.pickName);

        return result;
    }

    public bool AreAllSpawnersIdle()
    {
        return spawners.All(e => e.IsIdle());
    }

    public bool IsNotFinished()
    {
        return state != "finished";
    }
}
