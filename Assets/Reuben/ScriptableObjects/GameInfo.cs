using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameInfo")]
public class GameInfo : ScriptableObject
{
    public float score;

    public Dictionary<string, int> scores = new Dictionary<string, int>();
    public string currentPlayer;

}
