using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int chunkSize;

    private float chunkCenter;


    // Start is called before the first frame update
    void Start()
    {
        chunkCenter = chunkSize / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector2(chunkSize, chunkSize));
    }
}

