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
    [SerializeField] private AnimationCurve audioControllCurve;

    [Header("Perfect Swing")]
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private MMF_Player floatingTextMMFPlayer;
    [SerializeField] private int perfectSwingScoreIncrease = 100;

    [Header("Score Streak")]
    [SerializeField] private MMF_Player streakEndedMMFPlayer;
    [SerializeField] private float forceRequiredToEndStreak = 10f;
    private Coroutine scoreMultiplierCoroutine;
    private bool isStreaking = false;
    private int consecutiveSwings = 0;


    void OnEnable()
    {
        EventSystem.OnPerfectSwing += OnPerfectSwing;
        EventSystem.OnScoreStreakEnded += OnScoreStreakEnded;
    }

    void OnDisable()
    {
        EventSystem.OnPerfectSwing -= OnPerfectSwing;
        EventSystem.OnScoreStreakEnded -= OnScoreStreakEnded;
    }

    void OnPerfectSwing()
    {
        consecutiveSwings++;
        audioSource.PlayOneShot(scoreUpSound, .1f);
        gameInfo.score += perfectSwingScoreIncrease * consecutiveSwings;
        MMF_FloatingText floatingText = floatingTextMMFPlayer.GetFeedbackOfType<MMF_FloatingText>();
        floatingText.Value = "Perfect Swing! +" + perfectSwingScoreIncrease * consecutiveSwings;
        floatingTextMMFPlayer.PlayFeedbacks();
        if (consecutiveSwings >= 1)
        {
            isStreaking = true;
            gameUi.UpdateScoreMultiplier(consecutiveSwings);
        }
    }

    // IEnumerator ScoreMultiplier()
    // {
    //     while (true)
    //     {
    //         scoreMultiplier += Time.deltaTime / 6;
    //         float scoreIncrease = 1 * scoreMultiplier;
    //         gameInfo.score += scoreIncrease;
    //         gameUi.UpdateScoreMultiplier(scoreMultiplier);
    //         yield return null;
    //     }
    // }

    void OnScoreStreakEnded(Vector2 collisionForce, float maxVelocity)
    {
        if (!isStreaking) return;
        if (collisionForce.magnitude < forceRequiredToEndStreak) return;

        consecutiveSwings = 0;
        gameUi.UpdateScoreMultiplier(consecutiveSwings);
        isStreaking = false;

        streakEndedMMFPlayer.PlayFeedbacks();

        float minVol = .1f;
        float maxVol = 1f;
        float magnitudeToVol = Mathf.Clamp(collisionForce.magnitude, 0, maxVelocity);
        float scaledVol = Mathf.Lerp(minVol, maxVol, audioControllCurve.Evaluate(magnitudeToVol / maxVelocity));
        audioSource.volume = scaledVol;
        audioSource.PlayOneShot(scoreSreakEndSound, scaledVol);
    }
}
