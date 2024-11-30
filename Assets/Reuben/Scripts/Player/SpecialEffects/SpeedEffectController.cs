using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffectController : MonoBehaviour
{
    [SerializeField] private Transform player;

    void OnEnable()
    {
        EventSystem.OnBroadCastPlayerMovementDirection += OnBroadCastPlayerMovementDirection;
    }
    void OnDisable()
    {
        EventSystem.OnBroadCastPlayerMovementDirection -= OnBroadCastPlayerMovementDirection;
    }

    void Update()
    {
        transform.position = player.position;
    }

    void OnBroadCastPlayerMovementDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
