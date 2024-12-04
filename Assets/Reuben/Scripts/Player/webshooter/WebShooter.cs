using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;

public class WebShooter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [Header("Shooter References")]
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    [SerializeField] private GameObject shooterPivot;
    [SerializeField] private GameObject shooter;
    private RaycastHit2D shooterHit;
    private Vector2 mousePos;
    private Quaternion targetRotation;

    [Header("Web Settings")]
    [SerializeField] private float webRetractSpeed;
    [SerializeField] private float shooterAngleResetSpeed;
    [SerializeField] private float webRange;
    [SerializeField] private LayerMask layerMask;
    private bool shooting = false;
    float webDamping;
    float webFrequency;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip thudSound;
    [SerializeField] private AudioClip popSound;


    float breakForce;
    bool timeToClick = false;


    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
        webDamping = springJoint.dampingRatio;
        webFrequency = springJoint.frequency;


        rb = GetComponent<Rigidbody2D>();
    }
    Coroutine giveSpeedCoroutine;
    private void OnJointBreak2D(Joint2D joint)
    {
        //Debug.Log($"joint broken - reaction force = {springJoint.reactionForce}");
        if (giveSpeedCoroutine == null)
        {
            giveSpeedCoroutine = StartCoroutine(GiveSpeedBoost());
        }
    }

    IEnumerator GiveSpeedBoost()
    {
        yield return new WaitForSeconds(.5f);
        timeToClick = true;
        yield return new WaitForSeconds(1f);
        timeToClick = false;
        giveSpeedCoroutine = null;
    }



    void Update()
    {
        Debug.Log(springJoint.reactionForce.magnitude);
        mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)shooter.transform.position;

        if (shooting)
        {
            StopCoroutine(RotateToMouseLook(shooterPivot, direction));
            LookAt(shooterHit.point);
        }
        else 
        { 
            SlerpAndLockLookAt(shooterPivot, direction);
        }

        if (Input.GetMouseButtonDown(0) && CastRay() && !shooting)
        {
            shooting = true;
            StopCoroutine(RotateToMouseLook(shooterPivot, direction));
            LookAt(shooterHit.point);
            FireWeb();
            springJoint.dampingRatio = .5f;
            springJoint.frequency = .5f;
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

            if (timeToClick)
            {
                Debug.Log("SpeedBoostApplied");
                rb.AddForce(new Vector2(50, 0), ForceMode2D.Impulse);
            }
        }

        if (springJoint.enabled)
        {
            lineRenderer.SetPosition(1, shooter.transform.position);
        }
    }

    void FireWeb()
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        springJoint.connectedAnchor = shooterHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, shooter.transform.position);
        EventSystem.OnSendShooterHitPointInfo?.Invoke(shooterHit.point, transform.position);
    }

    bool CastRay()
    {
        Vector2 castDirection = (mousePos - (Vector2)shooter.transform.position).normalized;
        shooterHit = Physics2D.Raycast(shooter.transform.position, castDirection, webRange, layerMask);
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
        Vector2 direction = point - (Vector2)shooterPivot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shooterPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
