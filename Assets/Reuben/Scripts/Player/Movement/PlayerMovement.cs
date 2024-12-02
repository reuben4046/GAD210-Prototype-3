using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactParticles;

    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask layerMask;
    bool isGrounded;
    private float velocityMagnitude;
    [SerializeField] private int maxVelocity = 20;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource windAudioSource;
    [SerializeField] private AnimationCurve audioControllCurve;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip windWhooshSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Vector2 velocity = rb.velocity;
        velocityMagnitude = velocity.magnitude;
        rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;

        if (velocity.magnitude == 40)
        {
            audioSource.PlayOneShot(windWhooshSound);
        }
        // ScaleWindSoundBasedOnVelocity();
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
        Vector2 collisionForce = other.relativeVelocity;
        
        ScaleImpactSoundBasedOnVelocity(collisionForce);

        Vector2 contactPoint = other.GetContact(0).point;
        impactParticles.transform.position = contactPoint;
        impactParticles.Play();

        EventSystem.OnPlayerCollision?.Invoke(contactPoint, collisionForce.magnitude);
    }

    void ScaleImpactSoundBasedOnVelocity(Vector2 collisionForce)
    {
        float minVol = 0.001f;
        float maxVol = .5f;
        float velocityToVol = Mathf.Clamp(collisionForce.magnitude, 0, maxVelocity);
        float scaledVol = Mathf.Lerp(minVol, maxVol, audioControllCurve.Evaluate(velocityToVol / maxVelocity));
        audioSource.PlayOneShot(landingSound, scaledVol);
    }

    // void ScaleWindSoundBasedOnVelocity()
    // {
    //     float minVol = 0f;
    //     float maxVol = .5f;
    //     float velocityToVol = Mathf.Clamp(rb.velocity.magnitude, 0, maxVelocity);
    //     float scaledVol = Mathf.Lerp(minVol, maxVol, audioControllCurve.Evaluate(velocityToVol / maxVelocity));
    //     windAudioSource.volume = scaledVol;

    //     float minPitch = 0.1f;
    //     float maxPitch = 1f;
    //     float velocityToPitch = Mathf.Clamp(rb.velocity.magnitude, 0, maxVelocity);
    //     float scaledPitch = Mathf.Lerp(minPitch, maxPitch, audioControllCurve.Evaluate(velocityToPitch / maxVelocity));
    //     windAudioSource.pitch = scaledPitch;
    // }

}
