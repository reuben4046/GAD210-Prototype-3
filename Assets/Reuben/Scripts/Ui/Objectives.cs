using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    List<Transform> objectives = new List<Transform>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            objectives.Add(child);
        }
        EventSystem.OnChunkSpawned?.Invoke(objectives);
    }
}
