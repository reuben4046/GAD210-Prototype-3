using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Shooter : MonoBehaviour
{
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private DistanceJoint2D distanceJoint;

    [SerializeField] private GameObject shooterPivot;
    [SerializeField] private GameObject shooter;

    bool shooting = false;
    public float webRetractSpeed;
    public float grappleRetractSpeed;

    [SerializeField] private float webRange = 10f;

    RaycastHit2D shooterHit;
    Vector2 mousePos;

    enum ShotType
    {
        Web,
        Grapple
    }

    Quaternion targetRotation;

    float angleResetSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        springJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)shooter.transform.position;

        if (shooting)
        {
            StopAllCoroutines();
            LookAtShooterPoint();
        }
        else 
        { 
            SlerpAndLockLookAt(shooterPivot, direction);
        }

        if (Input.GetMouseButtonDown(0) && CastRay() && !shooting)
        {
            shooting = true;
            FireShot(ShotType.Web);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            shooting = false;
            CancelShot();
        }

        if (Input.GetMouseButtonDown(1) && CastRay() && !shooting)
        {
            shooting = true;
            FireShot(ShotType.Grapple);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            shooting = false;
            CancelShot();
        }

        if (distanceJoint.enabled || springJoint.enabled)
        {
            Contract(ShotType.Grapple);
            Contract(ShotType.Web);
            lineRenderer.SetPosition(1, shooter.transform.position);
        }
    }

    bool CastRay()
    {
        Vector2 castDirection = (mousePos - (Vector2)shooter.transform.position).normalized;
        int layersIncluded = 1 << 6;
        LayerMask layerMask = ~layersIncluded;
        shooterHit = Physics2D.Raycast(shooter.transform.position, castDirection, webRange, layerMask);
        if (shooterHit)
        {
            return true;
        }
        return false;
    }

    void FireShot(ShotType type)
    {
        switch (type)
        {
            case ShotType.Web:
                lineRenderer.SetPosition(0, shooterHit.point);
                lineRenderer.SetPosition(1, transform.position);
                springJoint.connectedAnchor = shooterHit.point;
                springJoint.enabled = true;
                lineRenderer.enabled = true;
                break;

            case ShotType.Grapple:
                lineRenderer.SetPosition(0, shooterHit.point);
                lineRenderer.SetPosition(1, transform.position);
                distanceJoint.connectedAnchor = shooterHit.point;
                distanceJoint.enabled = true;
                lineRenderer.enabled = true;
                break;
        }
    }

    void Contract(ShotType type)
    {
        switch (type)
        {
            case ShotType.Web:
                float distance = springJoint.distance;
                springJoint.distance = Mathf.Lerp(distance, 0, distance * webRetractSpeed * Time.deltaTime);
                break;

            case ShotType.Grapple:
                distanceJoint.distance -= Time.deltaTime * grappleRetractSpeed;
                break;
        }
    }

    void CancelShot()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    void LookAtShooterPoint()
    {
        Vector2 direction = shooterHit.point - (Vector2)shooterPivot.transform.position;
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
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation, Time.deltaTime * angleResetSpeed);
            yield return null;
        }

        obj.transform.rotation = targetRotation; // Snap the rotation when it's complete
    }

}
