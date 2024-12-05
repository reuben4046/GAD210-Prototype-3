using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class GameUi : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI consecutiveSwingsText;

    [SerializeField] private Slider timeTillPerfectSwingSlider;

    [SerializeField] private MMSpringTMPDilate scoreTextSpring;

    [SerializeField] private TextMeshProUGUI speedBonusText;

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
        scoreText.text = gameInfo.score.ToString("0");

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

    public void UpdateTexts(float consecutiveSwings, float streakEndPenalty)
    {
        scoreTextSpring.BumpRandom();
        if (streakEndPenalty > 0)
        {
            gameInfo.score -= streakEndPenalty;
        }
        consecutiveSwingsText.text = consecutiveSwings.ToString("0");
    }

    void OnScoreStreakEnded(Vector2 collisionForce, float maxVelocity)
    {
        consecutiveSwingsText.text = "";
    }

    public void UpdateSpeedBonus(float speedBonus, bool visible)
    {
        if (visible)
        {
            speedBonusText.gameObject.SetActive(true);
        }
        else
        {
            speedBonusText.gameObject.SetActive(false);
        }
        speedBonusText.text = "+" + speedBonus.ToString("0");
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
