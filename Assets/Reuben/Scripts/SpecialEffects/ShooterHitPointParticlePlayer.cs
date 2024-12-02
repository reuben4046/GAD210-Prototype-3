using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHitPointParticlePlayer : MonoBehaviour
{
    ParticleSystem muzzleFlashParticle;

    void Start()
    {
        muzzleFlashParticle = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        EventSystem.OnSendShooterHitPointInfo += OnSendShooterHitPointInfo;
    }

    void OnDisable()
    {
        EventSystem.OnSendShooterHitPointInfo -= OnSendShooterHitPointInfo;
    }

    void OnSendShooterHitPointInfo(Vector2 shooterHitPoint, Vector2 playerPosition)
    {
        transform.position = shooterHitPoint;
        muzzleFlashParticle.Play();
    }
}
