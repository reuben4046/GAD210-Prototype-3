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

    [SerializeField] private MMSpringTMPDilate scoreTextSpringDilate;
    [SerializeField] private TextMeshProUGUI speedBonusText;

    [SerializeField] private Texture2D redCursor;
    [SerializeField] private Texture2D purpleCursor;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip noWebPointSound;

    void Start()
    {
        gameInfo.score = 0;

        consecutiveSwingsText.text = "";
    }

    void OnEnable()
    {
        EventSystem.OnScoreStreakEnded += OnScoreStreakEnded;
        // EventSystem.OnSendTimeTillPerfectSwing += OnSendTimeTillPerfectSwing;
        EventSystem.OnNoWebPointFeedback += OnNoWebPointFeedback;
    }

    void OnDisable()
    {
        EventSystem.OnScoreStreakEnded -= OnScoreStreakEnded;
        // EventSystem.OnSendTimeTillPerfectSwing -= OnSendTimeTillPerfectSwing;
        EventSystem.OnNoWebPointFeedback -= OnNoWebPointFeedback;
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

    public void UpdateTexts(float consecutiveSwings, int streakEndPenalty)
    {
        scoreTextSpringDilate.BumpRandom();
        if (streakEndPenalty > 0)
        {
            float currentScore = gameInfo.score;
            currentScore -= streakEndPenalty;
            if (currentScore < 0) gameInfo.score = 0;
            else gameInfo.score -= streakEndPenalty;
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

    void OnNoWebPointFeedback()
    {
        StartCoroutine(ChangeCursor());
    }

    IEnumerator ChangeCursor()
    {
        Cursor.SetCursor(redCursor, Vector2.zero, CursorMode.Auto);
        audioSource.PlayOneShot(noWebPointSound);
        yield return new WaitForSeconds(0.15f);
        Cursor.SetCursor(purpleCursor, Vector2.zero, CursorMode.Auto);
    }

    // float savedSliderValue = 0f;
    // void OnSendTimeTillPerfectSwing(float timeTillPerfectSwing, bool isPerfectSwinging)
    // {        
    //     if (timeTillPerfectSwing > savedSliderValue)
    //     {
    //         timeTillPerfectSwingSlider.value = timeTillPerfectSwingSlider.maxValue - timeTillPerfectSwing;
    //     }
    // }

}
