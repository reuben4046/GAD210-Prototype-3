using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText.text = $"Score: {gameInfo.score}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
