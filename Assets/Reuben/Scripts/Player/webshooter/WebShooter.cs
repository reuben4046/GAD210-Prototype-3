using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpringJoint2D))]
[RequireComponent(typeof(PlayerMovement))]
public class WebShooter : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Web Shooter References")]
    [SerializeField] private GameObject webShooterPivot;
    [SerializeField] private GameObject webShooter;    
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private RaycastHit2D shooterHit;
    private Vector2 mousePos;
    private Quaternion targetRotation;

    [Header("Web Settings")]
    [SerializeField] private float webRetractSpeed;
    [SerializeField] private float shooterAngleResetSpeed;
    [SerializeField] private float webRange;
    [SerializeField] private LayerMask layerMask;
    private bool shooting = false;
    [SerializeField] private float webDampingValue = 0.5f;
    [SerializeField] private float webFrequencyValue = 0.5f;

    [Header("Perfect Swing Settings")]
    [SerializeField] private float minPerfectSwingReactionAngle = -50f, maxPerfectSwingReactionAngle = -30f;
    [SerializeField] private AnimationCurve perfectSwingForceCurve;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip thudSound;
    [SerializeField] private AudioClip popSound;


    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void PerfectSwingController()
    {
        float reactionForceAngle = Mathf.Atan2(springJoint.reactionForce.y, springJoint.reactionForce.x) * Mathf.Rad2Deg; 
        if (reactionForceAngle >= minPerfectSwingReactionAngle && reactionForceAngle <= maxPerfectSwingReactionAngle)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ApplyPerfectSwingForce(); 
            }
        }
    }
    void ApplyPerfectSwingForce()
    {
        float minForce = 20f;
        float maxForce = 60f;
        float velocityToForce = Mathf.Clamp(playerMovement.velocityMagnitude, 0, playerMovement.maxVelocity);
        float scaledForce = Mathf.Lerp(minForce, maxForce, perfectSwingForceCurve.Evaluate(velocityToForce / playerMovement.maxVelocity));
        playerMovement.rb.AddForce(new Vector2(scaledForce, 0), ForceMode2D.Impulse);
        Debug.Log($"SpeedBoostApplied = {scaledForce}");
        EventSystem.OnPerfectSwing?.Invoke();
    }

    void Update()
    {
        // PerfectSwingController();
        mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)webShooter.transform.position;

        if (shooting)
        {
            StopCoroutine(RotateToMouseLook(webShooterPivot, direction));
            LookAt(shooterHit.point);
        }
        else 
        { 
            SlerpAndLockLookAt(webShooterPivot, direction);
        }

        if (Input.GetMouseButtonDown(0) && CastRay() && !shooting)
        {
            shooting = true;
            StopCoroutine(RotateToMouseLook(webShooterPivot, direction));
            LookAt(shooterHit.point);
            FireWeb();
            audioSource.PlayOneShot(thudSound);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (shooting)
            {
                audioSource.PlayOneShot(thudSound, .3f);
                EventSystem.OnSendShooterHitPointInfo?.Invoke(shooterHit.point, transform.position);
            }
            shooting = false;
            CancelWeb();
        }

        if (springJoint.enabled)
        {
            lineRenderer.SetPosition(1, webShooter.transform.position);
        }
    }

    void FireWeb()
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        springJoint.connectedAnchor = shooterHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, webShooter.transform.position);
        EventSystem.OnSendShooterHitPointInfo?.Invoke(shooterHit.point, transform.position);
    }

    bool CastRay()
    {
        Vector2 castDirection = (mousePos - (Vector2)webShooter.transform.position).normalized;
        shooterHit = Physics2D.Raycast(webShooter.transform.position, castDirection, webRange, layerMask);
        if (shooterHit)
        {
            return true;
        }
        return false;
    }

    void ContractWeb()
    {
        float distance = springJoint.distance;
        springJoint.distance = Mathf.Lerp(distance, 0, distance * webRetractSpeed * Time.deltaTime);
    }

    void CancelWeb()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    void LookAt(Vector2 point)
    {
        Vector2 direction = point - (Vector2)webShooterPivot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        webShooterPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void SlerpAndLockLookAt(GameObject obj, Vector2 direction)
    {
        StartCoroutine(RotateToMouseLook(obj, direction));
    }

    IEnumerator RotateToMouseLook(GameObject obj, Vector2 direction)
    {
        direction = mousePos - (Vector2)obj.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        while (Quaternion.Angle(obj.transform.rotation, targetRotation) > 1f)
        {
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation, Time.deltaTime * shooterAngleResetSpeed);
            yield return null;
        }

        obj.transform.rotation = targetRotation;
    }

}
