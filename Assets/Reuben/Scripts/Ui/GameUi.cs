using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private TextMeshProUGUI distanceTraveledText;

    [SerializeField] private TextMeshProUGUI consecutiveSwingsText;

    [SerializeField] private Slider timeTillPerfectSwingSlider;

    void Start()
    {
        gameInfo.score = 0;

        consecutiveSwingsText.text = "";
    }

    void OnEnable()
    {
        EventSystem.OnScoreStreakEnded += OnScoreStreakEnded;
        EventSystem.OnSendTimeTillPerfectSwing += OnSendTimeTillPerfectSwing;
    }

    void OnDisable()
    {
        EventSystem.OnScoreStreakEnded -= OnScoreStreakEnded;
        EventSystem.OnSendTimeTillPerfectSwing -= OnSendTimeTillPerfectSwing;
    }

    void Update()
    {
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

    public void UpdateScoreMultiplier(float consecutiveSwings)
    {
        consecutiveSwingsText.text = consecutiveSwings.ToString("0");
    }

    void OnScoreStreakEnded(Vector2 collisionForce, float maxVelocity)
    {
        consecutiveSwingsText.text = "";
    }

    float savedSliderValue = 0f;
    void OnSendTimeTillPerfectSwing(float timeTillPerfectSwing, bool isPerfectSwinging)
    {        
        if (timeTillPerfectSwing > savedSliderValue)
        {
            timeTillPerfectSwingSlider.value = timeTillPerfectSwingSlider.maxValue - timeTillPerfectSwing;
        }

    }
}
