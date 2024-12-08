using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelBounds : MonoBehaviour
{
    private Transform playerTransform;
    private Vector2 position;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    
    void Update()
    {
        position = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = position;
    }
}
