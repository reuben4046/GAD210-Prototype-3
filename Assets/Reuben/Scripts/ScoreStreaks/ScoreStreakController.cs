using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ScoreStreakController : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameUi gameUi;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip scoreUpSound;
    [SerializeField] private AudioClip scoreSreakEndSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AnimationCurve audioControllCurve;

    [Header("Perfect Swing")]
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private MMF_Player floatingTextMMFPlayer;
    [SerializeField] private int perfectSwingScoreIncrease = 100;
    private int consecutiveSwings = 0;
    MMF_FloatingText perfectSwingFloatingText;

    [Header("Score Streak")]
    [SerializeField] private MMF_Player streakEndedMMFPlayer;
    [SerializeField] private float forceRequiredToEndStreak = 10f;
    private Coroutine scoreMultiplierCoroutine;
    private bool isStreaking = false;
    [SerializeField] private AnimationCurve streakEndPenaltyCurve;
    [SerializeField] private float maxStreakEndPenalty = 300f;
    [SerializeField] private float minStreakEndPenalty = 100f;
    private float streakEndPenalty;

    [Header("Speed Bonus")]
    float displayedSpeedBonusNumber = 0f;
    void OnEnable()
    {
        EventSystem.OnPerfectSwing += OnPerfectSwing;
        EventSystem.OnScoreStreakEnded += OnScoreStreakEnded;
        EventSystem.OnSpeedBonusActive += OnSpeedBonusActive;
    }

    void OnDisable()
    {
        EventSystem.OnPerfectSwing -= OnPerfectSwing;
        EventSystem.OnScoreStreakEnded -= OnScoreStreakEnded;
        EventSystem.OnSpeedBonusActive -= OnSpeedBonusActive;
    }
    
    void Start()
    {
        perfectSwingFloatingText = floatingTextMMFPlayer.GetFeedbackOfType<MMF_FloatingText>();
        StartCoroutine(SpeedBonus());
    }

    void OnPerfectSwing()
    {
        consecutiveSwings++;
        audioSource.PlayOneShot(scoreUpSound, .1f);
        gameInfo.score += perfectSwingScoreIncrease * consecutiveSwings;
        perfectSwingFloatingText.Value = "Perfect Swing! +" + perfectSwingScoreIncrease * consecutiveSwings;
        floatingTextMMFPlayer.PlayFeedbacks();
        gameUi.UpdateTexts(consecutiveSwings, 0);
        if (consecutiveSwings >= 1)
        {
            isStreaking = true;
        }
    }

    void OnScoreStreakEnded(Vector2 collisionForce, float maxVelocity)
    {
        if (!isStreaking) return;
        if (collisionForce.magnitude < forceRequiredToEndStreak) return;

        consecutiveSwings = 0;

        float collisionForceToStreakEndPenalty = Mathf.Clamp(collisionForce.magnitude, 0, maxVelocity);
        float scaledStreakEndPenalty = Mathf.Lerp(minStreakEndPenalty, maxStreakEndPenalty, streakEndPenaltyCurve.Evaluate(collisionForceToStreakEndPenalty / maxVelocity));
        streakEndPenalty = scaledStreakEndPenalty;
        gameUi.UpdateTexts(consecutiveSwings, (int)streakEndPenalty);
        perfectSwingFloatingText.Value = "-" + (int)streakEndPenalty;
        isStreaking = false;

        streakEndedMMFPlayer.PlayFeedbacks();
        floatingTextMMFPlayer.PlayFeedbacks();

        float minVol = .1f;
        float maxVol = 1f;
        float magnitudeToVol = Mathf.Clamp(collisionForce.magnitude, 0, maxVelocity);
        float scaledVol = Mathf.Lerp(minVol, maxVol, audioControllCurve.Evaluate(magnitudeToVol / maxVelocity));
        audioSource.volume = scaledVol;
        audioSource.PlayOneShot(scoreSreakEndSound, scaledVol);
    }

    bool speedBonusOn = false;
    void OnSpeedBonusActive(bool isSpeedBonusActive)
    {
        if (isSpeedBonusActive)
        {
            speedBonusOn = true;
        }
        else
        {
            speedBonusOn = false;
        }
    }

    IEnumerator SpeedBonus()
    {
        while (true)
        {
            if (speedBonusOn)
            {
                displayedSpeedBonusNumber++;
                gameUi.UpdateSpeedBonus(displayedSpeedBonusNumber, visible: true);
                gameInfo.score++;
                audioSource.PlayOneShot(clickSound, .3f);
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                displayedSpeedBonusNumber = 0f;
                gameUi.UpdateSpeedBonus(displayedSpeedBonusNumber, visible: false);
                yield return null;
            }
        }
    }
}
