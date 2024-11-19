using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTarget : MonoBehaviour
{
    private Transform player;

    private float cameraPosX = 0f;

    private float cameraMovementSpeed = 0f;

    [Range(0f, .5f)]
    [SerializeField] private float speedMultiplier = 0.2f;
    [SerializeField] private float maxCameraMovementSpeed = 100f;
    [SerializeField] private float catchUpToPlayerOffset = 20f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraMovementSpeed += Time.deltaTime;
        cameraPosX += cameraMovementSpeed * Time.deltaTime * speedMultiplier;
        Mathf.Clamp(cameraMovementSpeed, 0, maxCameraMovementSpeed);

        if (transform.position.x <= player.position.x - catchUpToPlayerOffset - 1f)
        {
            cameraPosX = player.position.x - catchUpToPlayerOffset;
        }     

        transform.position = new Vector2(cameraPosX, player.position.y);

        if (transform.position.x > player.position.x + 40f || transform.position.y < -80f)
        {
            SceneManager.LoadScene(2);
            EventSystem.OnGameOver?.Invoke();
            Debug.Log("Game Over");
        }
    }
}
