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
        Vector2 direction = (shooterHitPoint - playerPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;

        muzzleFlashParticle.Play();
    }
}
