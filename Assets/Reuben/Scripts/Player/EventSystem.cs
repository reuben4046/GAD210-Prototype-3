using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class EventSystem
{
    public delegate void PlayerHitGround();
    public static PlayerHitGround OnPlayerHitGround;

    public delegate void ChunkSpawned(List<Transform> objectives);
    public static ChunkSpawned OnChunkSpawned;

    public delegate void GameOver();
    public static GameOver OnGameOver;
}
