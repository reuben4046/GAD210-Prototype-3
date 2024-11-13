using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public static ChunkSpawner instance;

    [SerializeField] private Transform player;

    [SerializeField] private int chunkSize;

    private float chunkCenter;

    List<Chunk> chunks = new List<Chunk>();

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
        int randomChunk = Random.Range(0, chunks.Count);
        Chunk chunk = Instantiate(chunks[randomChunk], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        chunk.transform.parent = transform;
        foreach (Chunk c in chunks)
        {
            if (c != chunk)
            {
                c.isMostRecentChunk = false;
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector2(chunkSize, chunkSize));
    }
}

