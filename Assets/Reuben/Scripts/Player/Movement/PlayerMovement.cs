using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask layerMask;
    AudioSource soundOutput;
    public AudioClip jump;
    public AudioClip landing;


    public int maxVelocity = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soundOutput = GetComponent<AudioSource>();
    }
    bool isGrounded;
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            soundOutput.PlayOneShot(jump);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (IsGrounded())
        {
            if (!isGrounded)
            {
                isGrounded = true;
                soundOutput.PlayOneShot(landing);
                EventSystem.OnPlayerHitGround?.Invoke();
            }
        }
        else
        {
            isGrounded = false;
        }

        Vector2 velocity = rb.velocity;
        float velocityMagnitude = velocity.magnitude;
        rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;
    }

    void FixedUpdate()
    {   
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * speed * Time.fixedDeltaTime);
        }

        
        // Vector2 velocity = rb.velocity;
        // float velocityMagnitude = velocity.magnitude;

        // rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;
    }

    private bool IsGrounded()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(layerMask);
    }
}
