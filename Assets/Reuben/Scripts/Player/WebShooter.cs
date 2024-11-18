using System.Collections;
using UnityEngine;

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

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
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
            FireShot(direction);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            shooting = false;
            CancelWeb();
        }

        if (springJoint.enabled)
        {
            ContractWeb();
            lineRenderer.SetPosition(1, shooter.transform.position);
        }
    }

    void FireShot(Vector2 direction)
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        lineRenderer.SetPosition(1, transform.position);
        springJoint.connectedAnchor = shooterHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
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
