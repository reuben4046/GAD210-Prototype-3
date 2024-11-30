using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
{
    [SerializeField] private List<GameObject> splatterSprites = new List<GameObject>();

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
        Instantiate(splatterSprites[randomIndex], contactPoint, Quaternion.identity);
    }

    void Update()
    {
        if (splatterSprites.Count > 10)
        {
            while (splatterSprites.Count > 10)
            {
                Destroy(splatterSprites[0]);
                splatterSprites.RemoveAt(0);
            }
        }
    }
}
