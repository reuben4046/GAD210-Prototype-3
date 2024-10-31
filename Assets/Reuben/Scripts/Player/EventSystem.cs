using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class EventSystem
{
    public delegate void PlayerHitGround();
    public static PlayerHitGround OnPlayerHitGround;
}
