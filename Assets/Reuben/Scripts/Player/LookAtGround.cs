using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtGround : MonoBehaviour
{
    GameObject playerSprite;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.down);
    }

    void OnEnable()
    {
        EventSystem.OnPlayerHitGround += OnPlayerHitGround;
    }

    void OnDisable()
    {
        EventSystem.OnPlayerHitGround -= OnPlayerHitGround;
    }

    void OnPlayerHitGround()
    {
        Debug.Log("OnPlayerHitGround");
        LeanTween.scaleY(gameObject, 0.5f, 0.5f)
        .setOnComplete(() => LeanTween.scaleY(playerSprite, 1f, 0.5f));
    }

}
