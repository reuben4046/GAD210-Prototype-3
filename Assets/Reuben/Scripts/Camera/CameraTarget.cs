using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform player;

    private float forwardMovement = 0f;

    [SerializeField] private float cameraMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        forwardMovement += cameraMovementSpeed * Time.deltaTime;

        if (transform.position.x < player.position.x - 20f)
        {
            forwardMovement = player.position.x - 20f;
        }     

        transform.position = new Vector2(forwardMovement, player.position.y);
    }
}
