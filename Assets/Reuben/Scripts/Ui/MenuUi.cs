using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    [SerializeField] private GameObject scoreEntryPrefab;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    Transform VerticalLayoutGroupTransform { get { return verticalLayoutGroup.transform; } }

    void Start()
    {
        if (gameInfo.currentPlayer != null) inputField.text = gameInfo.currentPlayer;
        ClearScores();
        RefreshScores();
    }

    public void RefreshScores()
    {
        if (gameInfo.scores.Count == 0) return;
        var top5 = gameInfo.scores.OrderByDescending(score => score.Value).Take(5);
        int placeNumber = 0;
        foreach (var score in top5)
        {
            var scoreEntry = Instantiate(scoreEntryPrefab, VerticalLayoutGroupTransform);
            placeNumber++;
            scoreEntry.GetComponentInChildren<TextMeshProUGUI>().text = $"#{placeNumber} {score.Key}: {score.Value}";
        }
    }

    public void ClearScores()
    {
        if (gameInfo.scores.Count == 0) return;
        foreach (Transform child in VerticalLayoutGroupTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        if (inputField.text.Length == 0) 
        {
            StartCoroutine(SetColor());
            return;
        }
        if (gameInfo.scores.ContainsKey(inputField.text))
        {
            gameInfo.currentPlayer = inputField.text;
            SceneManager.LoadScene(1);
            return;
        }
        gameInfo.scores.Add(inputField.text, 0);
        gameInfo.currentPlayer = inputField.text;
        SceneManager.LoadScene(1);
    }

    IEnumerator SetColor()
    {
        Color originalColor = inputField.placeholder.color;
        inputField.placeholder.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        inputField.placeholder.color = originalColor;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
