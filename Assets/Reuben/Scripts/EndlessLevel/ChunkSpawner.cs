using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public static ChunkSpawner instance;

    [SerializeField] private Transform player;

    [SerializeField] private int chunkSize;

    private float chunkCenter;
    [SerializeField] private GameObject[] chunks;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        chunkCenter = chunkSize / 2;
        GenerateChunk();
    }

    void GenerateChunk()
    {
        int randomChunk = Random.Range(0, chunks.Length);
        GameObject chunk = Instantiate(chunks[randomChunk], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        chunk.transform.parent = transform;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector2(chunkSize, chunkSize));
    }
}

