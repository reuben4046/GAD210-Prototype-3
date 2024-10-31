using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtGround : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.down);
    }

    void OnEnable()
    {
        OnPlayerHitGround += OnPlayerHitGround;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHitGround -= OnPlayerHitGround;
    }

    void OnPlayerHitGround()
}
