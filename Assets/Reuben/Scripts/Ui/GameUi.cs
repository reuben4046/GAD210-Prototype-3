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
    }
}
