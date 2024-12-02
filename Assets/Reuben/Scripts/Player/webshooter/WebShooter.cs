using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;

public class WebShooter : MonoBehaviour
{
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
    // AudioSource soundOutput;
    // public AudioClip webSound;



    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
        // soundOutput = GetComponent<AudioSource>();
        webDamping = springJoint.dampingRatio;
        webFrequency = springJoint.frequency;
    }

    void Update()
    {
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
            // soundOutput.volume = 0.5f;
            // soundOutput.PlayOneShot(webSound);
            FireShot();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            shooting = false;
            CancelWeb();
        }

        if (Input.GetMouseButton(1))
        {
            springJoint.dampingRatio = .5f; //////Could possibly make it so it is like this all the time. need to ask cooper what he thinks when I see him next
            springJoint.frequency = .5f;
            
        } 
        else
        {
            ContractWeb();
            springJoint.dampingRatio = webDamping;
            springJoint.frequency = webFrequency;
        }

        if (springJoint.enabled)
        {
            lineRenderer.SetPosition(1, shooter.transform.position);
        }
    }

    void FireShot()
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        lineRenderer.SetPosition(1, transform.position);
        springJoint.connectedAnchor = shooterHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
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
