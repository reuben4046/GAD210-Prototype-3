using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
{
    [SerializeField] private List<GameObject> splatterSprites = new List<GameObject>();
    private List<GameObject> spawnedSplatters = new List<GameObject>();

    [SerializeField] private int maxSplattersInScene = 15;

    [SerializeField] private AnimationCurve splatterScaleCurve;
    [SerializeField] private float minSplatterScale = 0.1f;
    [SerializeField] private float maxSplatterScale = 2f;

    void OnEnable()
    {
        EventSystem.OnPlayerCollision += OnPlayerCollision;
    }

    void OnDisable()
    {
        EventSystem.OnPlayerCollision -= OnPlayerCollision;
    }

    void OnPlayerCollision(Vector2 contactPoint, float collisionForce)
    {
        SpawnSplatter(contactPoint, collisionForce);
    }

    void SpawnSplatter(Vector2 contactPoint, float collisionForce)
    {
        int randomIndex = Random.Range(0, splatterSprites.Count);
        GameObject splatter = Instantiate(splatterSprites[randomIndex], contactPoint, Quaternion.identity);
        float splatterScale = Mathf.Lerp(minSplatterScale, maxSplatterScale, splatterScaleCurve.Evaluate(collisionForce / 50f));
        splatter.transform.localScale = new Vector2(splatterScale, splatterScale);
        spawnedSplatters.Add(splatter);
    }

    void Update()
    {
        if (spawnedSplatters.Count > maxSplattersInScene)
        {
            Destroy(spawnedSplatters[0]);
            spawnedSplatters.RemoveAt(0);
        }
    }
}
