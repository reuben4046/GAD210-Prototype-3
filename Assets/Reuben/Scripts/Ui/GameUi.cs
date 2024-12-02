using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUi : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    private Transform playerTransform;
    [SerializeField] private TextMeshProUGUI distanceTraveledText;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        distanceTraveledText.text = playerTransform.position.x.ToString("0");
        gameInfo.score = (int)playerTransform.position.x;
        if (gameInfo.scores.ContainsKey(gameInfo.currentPlayer))
        {
            if (gameInfo.score > gameInfo.scores[gameInfo.currentPlayer])
            {
                gameInfo.scores[gameInfo.currentPlayer] = gameInfo.score;
            }
        }
        else
        {
            gameInfo.scores[gameInfo.currentPlayer] = gameInfo.score;
        }

        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
