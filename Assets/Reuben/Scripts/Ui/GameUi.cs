using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUi : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    private Transform playerTransform;
    [SerializeField] private TextMeshProUGUI distanceTraveledText;
    private int previousX = 0;

    [SerializeField] private TextMeshProUGUI scoreMultiplierText;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;

        gameInfo.score = 0;

        scoreMultiplierText.text = "";
    }

    void OnEnable()
    {
        EventSystem.OnScoreStreakEnded += OnScoreStreakEnded;
    }

    void OnDisable()
    {
        EventSystem.OnScoreStreakEnded -= OnScoreStreakEnded;
    }

    void Update()
    {
        if ((int)playerTransform.position.x > previousX)
        {
            gameInfo.score ++;
            previousX = (int)playerTransform.position.x;
        }

        distanceTraveledText.text = gameInfo.score.ToString("0");

        if (gameInfo.scores.ContainsKey(gameInfo.currentPlayer))
        {
            if (gameInfo.score > gameInfo.scores[gameInfo.currentPlayer])
            {
                gameInfo.scores[gameInfo.currentPlayer] = (int)gameInfo.score;
            }
        }
        else
        {
            gameInfo.scores[gameInfo.currentPlayer] = (int)gameInfo.score;
        }

        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateScoreMultiplier(float scoreMultiplier)
    {
        if (scoreMultiplier == 1f) scoreMultiplierText.text = "";
        scoreMultiplierText.text = scoreMultiplier.ToString("0.0") + "x";
    }

    void OnScoreStreakEnded(Vector2 collisionForce, float maxVelocity)
    {
        scoreMultiplierText.text = "";
    }
}
