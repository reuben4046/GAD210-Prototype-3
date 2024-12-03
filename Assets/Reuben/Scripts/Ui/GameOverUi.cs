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
        scoreText.text = $"Score: {(int)gameInfo.score}";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
