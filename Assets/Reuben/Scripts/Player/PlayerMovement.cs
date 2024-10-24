using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public int maxVelocity = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * speed);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float velocityMagnitude = velocity.magnitude;

        if (velocityMagnitude > maxVelocity)
        {
            Debug.Log("Velocity too high: " + velocityMagnitude);
            Vector2 velocityDirection = velocity.normalized;
            rb.velocity = velocityDirection * maxVelocity;
        }
    }

    private bool IsGrounded()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
