using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void OnPlayerHitGround();
    public static OnPlayerHitGround onPlayerHitGround;
}
