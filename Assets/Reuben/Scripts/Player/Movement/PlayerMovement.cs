using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactParticles;

    public Rigidbody2D rb;
    private LineRenderer lineRenderer;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private LayerMask layerMask;
    public float velocityMagnitude;
    private Vector2 collisionForce;
    public int maxVelocity = 20;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    // [SerializeField] private AudioSource windAudioSource;
    // [SerializeField] private AnimationCurve windAudioControllCurve;    
    [SerializeField] private AnimationCurve audioControllCurve;
    // [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip windWhooshSound;
    private bool whooshSoundCoolDown = false;
    [SerializeField] private float windWhooshSoundCoolDown = 1f;

    //ScoreStreakVariables
    private bool scoreStreakActive = false;
    int consecutiveSwings = 1;
    [SerializeField] private float perfectSwingThreshold = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            audioSource.PlayOneShot(landingSound, .3f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Vector2 velocity = rb.velocity;
        velocityMagnitude = velocity.magnitude;
        rb.velocity = Mathf.Clamp(velocityMagnitude, 0, maxVelocity) * velocity.normalized;

        if (velocityMagnitude > maxVelocity - perfectSwingThreshold && lineRenderer.enabled && !whooshSoundCoolDown)
        {
            EventSystem.OnPerfectSwing?.Invoke();
            whooshSoundCoolDown = true;
            StartCoroutine(WhooshSoundEffectCoolDown());
            audioSource.PlayOneShot(windWhooshSound, .3f);
        }
    }

    IEnumerator WhooshSoundEffectCoolDown()
    {
        yield return new WaitForSeconds(windWhooshSoundCoolDown);
        whooshSoundCoolDown = false;
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
        collisionForce = other.relativeVelocity;
        
        ScaleImpactSoundBasedOnVelocity(collisionForce);

        Vector2 contactPoint = other.GetContact(0).point;
        impactParticles.transform.position = contactPoint;
        impactParticles.Play();

        EventSystem.OnPlayerCollision?.Invoke(contactPoint, collisionForce.magnitude);
        EventSystem.OnScoreStreakEnded?.Invoke(collisionForce, maxVelocity);
        scoreStreakActive = false;
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
