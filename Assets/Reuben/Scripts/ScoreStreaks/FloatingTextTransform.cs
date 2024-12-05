using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextTransform : MonoBehaviour
{
    Transform target;

    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
