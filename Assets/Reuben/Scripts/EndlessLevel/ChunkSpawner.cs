using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int chunkSize;

    private float chunkCenter;

    [SerializeField] private List<Chunk> chunks = new List<Chunk>();
    private List<Chunk> chunksInScene = new List<Chunk>();
    
    Chunk mostRecentChunk;

    void Awake()
    {
        chunkCenter = chunkSize / 2;
        GenerateChunk(transform.position.x + chunkSize);
    }

    void Update()
    {
        SpawnChunks();
        DestroyOldChunks();
    }

    void SpawnChunks()
    {
        if (player.position.x > mostRecentChunk.transform.position.x)
        {
            float spawnPositionX = mostRecentChunk.transform.position.x + chunkSize;
            GenerateChunk(spawnPositionX);
        }
    }

    void GenerateChunk(float positionX)
    {
        int randomChunk = Random.Range(0, chunks.Count);
        mostRecentChunk = Instantiate(chunks[randomChunk], new Vector2(positionX, transform.position.y), Quaternion.identity);
        mostRecentChunk.transform.localScale = new Vector2(chunkSize, chunkSize);
        mostRecentChunk.transform.parent = transform;

        foreach (Chunk c in chunks)
        {
            c.isMostRecentChunk = false;
        }

        mostRecentChunk.isMostRecentChunk = true;
        chunksInScene.Add(mostRecentChunk);
    }

    void DestroyOldChunks()
    {
        if (chunksInScene.Count > 4)
        {
            Chunk oldestChunk = chunksInScene[0];
            chunksInScene.Remove(oldestChunk);
            Destroy(oldestChunk.gameObject);
        }
    }

}

