using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public int maxVelocity = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    bool isGrounded;
    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (IsGrounded())
        {
            if (!isGrounded)
            {
                isGrounded = true;
                EventSystem.OnPlayerHitGround?.Invoke();
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * speed * Time.deltaTime);
        }

        Vector2 velocity = rb.velocity;
        float velocityMagnitude = velocity.magnitude;

        rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;
    }

    private bool IsGrounded()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
