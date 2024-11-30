using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private Material playerHDRMat;
    [SerializeField] private AnimationCurve velocityColorCurve;

    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask layerMask;
    bool isGrounded;
    private float velocityMagnitude;
    [SerializeField] private int maxVelocity = 20;

    AudioSource soundOutput;
    public AudioClip jump;
    public AudioClip landing;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soundOutput = GetComponent<AudioSource>();
    }

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
        velocityMagnitude = velocity.magnitude;
        rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;

        ControlColorBasedOnVelocity();
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
    }

    private bool IsGrounded()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(layerMask);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 contactPoint = other.GetContact(0).point;
        impactParticles.transform.position = contactPoint;
        impactParticles.Play();
        EventSystem.OnPlayerCollision?.Invoke(contactPoint);
    }

    void ControlColorBasedOnVelocity()
    {
        //playerHDRMat.SetFloat("_Intensity", .78f - velocityColorCurve.Evaluate(velocityMagnitude / 100f));
    }
}
