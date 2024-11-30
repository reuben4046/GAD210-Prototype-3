using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private MMFeedbacks onPlayerCollision;

    void OnEnable()
    {
        EventSystem.OnPlayerCollision += OnPlayerCollision;
    }

    void OnDisable()
    {
        EventSystem.OnPlayerCollision -= OnPlayerCollision;
    }

    void OnPlayerCollision()
    {
        onPlayerCollision.PlayFeedbacks();
    } 
}
