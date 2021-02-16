using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensController : MonoBehaviour
{
    public static ChickensController instance;

    [SerializeField] List<ChickenController> chickens;
    [SerializeField] Transform[] chickenSpawners;
    [SerializeField] GameObject chickenPrefab;

    void Awake()
    {
        instance = this;
        chickens = new List<ChickenController>();
    }

    public void SendChickenToScene()
    {
        var randomSpawner = chickenSpawners[Random.Range(0, chickenSpawners.Length)];
        ChickenController chicken = Instantiate(chickenPrefab, randomSpawner.position, Quaternion.identity).GetComponent<ChickenController>();
        
        chicken.SetSpritesRenderOrder(chickens.Count);
        chickens.Add(chicken);

        chicken.GoToScene();
    }

    public void SendChickensOutOfScene()
    {
        foreach (var chicken in chickens)
        {
            chicken.GoToOriginalPosition();
        }

        chickens.Clear();
    }
}
