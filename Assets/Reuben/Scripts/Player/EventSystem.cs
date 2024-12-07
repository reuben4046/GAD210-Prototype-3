using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class EventSystem
{
    public delegate void ChunkSpawned(List<Transform> objectives);
    public static ChunkSpawned OnChunkSpawned;

    public delegate void GameOver();
    public static GameOver OnGameOver;

    public delegate void BroadCastPlayerMovementDirection(Vector2 direction);
    public static BroadCastPlayerMovementDirection OnBroadCastPlayerMovementDirection;

    public delegate void SendShooterHitPointInfo(Vector2 hitPoint, Vector2 playerPosition);
    public static SendShooterHitPointInfo OnSendShooterHitPointInfo;

    public delegate void PlayerCollision(Vector2 contactPoint, float collisionForce);
    public static PlayerCollision OnPlayerCollision;

    public delegate void PerfectSwing();
    public static PerfectSwing OnPerfectSwing;

    public delegate void ScoreStreakEnded(Vector2 collisionForce, float maxVelocity);
    public static ScoreStreakEnded OnScoreStreakEnded;

    public delegate void SendTimeTillPerfectSwing(float timeTillPerfectSwing, bool isPerfectSwinging);
    public static SendTimeTillPerfectSwing OnSendTimeTillPerfectSwing;

    public delegate void SpeedBonusActive(bool active);
    public static SpeedBonusActive OnSpeedBonusActive;

    public delegate void NoWebPointFeedback();
    public static NoWebPointFeedback OnNoWebPointFeedback;
}
