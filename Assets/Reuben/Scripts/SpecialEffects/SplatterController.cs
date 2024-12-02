using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
{
    [SerializeField] private List<GameObject> splatterSprites = new List<GameObject>();
    private List<GameObject> spawnedSplatters = new List<GameObject>();

    void OnEnable()
    {
        EventSystem.OnPlayerCollision += OnPlayerCollision;
    }

    void OnDisable()
    {
        EventSystem.OnPlayerCollision -= OnPlayerCollision;
    }

    void OnPlayerCollision(Vector2 contactPoint)
    {
        SpawnSplatter(contactPoint);
    }

    void SpawnSplatter(Vector2 contactPoint)
    {
        int randomIndex = Random.Range(0, splatterSprites.Count);
        GameObject splatter = Instantiate(splatterSprites[randomIndex], contactPoint, Quaternion.identity);
        spawnedSplatters.Add(splatter);
    }

    void Update()
    {
        if (spawnedSplatters.Count > 10)
        {
            Destroy(spawnedSplatters[0]);
            spawnedSplatters.RemoveAt(0);
        }
    }
}
