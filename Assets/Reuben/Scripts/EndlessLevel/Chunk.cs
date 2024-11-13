using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public bool isMostRecentChunk = true;

    [SerializeField] private float chunkCentre;

    Vector2 pos;
    Vector2 pos2;

    private void Start()
    {
        pos = new Vector2(transform.localScale.x/2f, -0.2f);
        pos2 = new Vector2(transform.localScale.x, -0.2f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos);
    }
}
