using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private DistanceJoint2D distanceJoint;

    [SerializeField] private GameObject ShooterPivot;
    [SerializeField] private GameObject shooter;

    bool shooting = false;
    public float webRetractSpeed;
    public float grappleRetractSpeed;

    [SerializeField] private float webRange = 10f;

    RaycastHit2D shooterHit;
    Vector2 mousePos;


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
        LookAtMouse(ShooterPivot);

        if (Input.GetMouseButtonDown(0))
        {
            if (CastRay() && !shooting)
            {
                shooting = true;
                FireWeb();
            }
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

        if (Input.GetMouseButtonDown(1))
        {
            if (CastRay() && !shooting)
            {
                shooting = true;
                FireGrapple();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            shooting = false;
            CancelGrapple();
        }
        if (distanceJoint.enabled)
        {
            ContractGrapple();
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
            Debug.Log(shooterHit.collider.name);
            return true;
        }
        Debug.Log("No hit");
        return false;
    }

    void FireWeb()
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        lineRenderer.SetPosition(1, transform.position);
        springJoint.connectedAnchor = shooterHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
    }
    void FireGrapple()
    {
        lineRenderer.SetPosition(0, shooterHit.point);
        lineRenderer.SetPosition(1, transform.position);
        distanceJoint.connectedAnchor = shooterHit.point;
        distanceJoint.enabled = true;
        lineRenderer.enabled = true;
    }

    void ContractWeb()
    {
        float distance = springJoint.distance;
        springJoint.distance = Mathf.Lerp(distance, 0, distance * webRetractSpeed * Time.deltaTime);
        //springJoint.distance -= Time.deltaTime * webRetractSpeed;

    }
    void ContractGrapple()
    {
        distanceJoint.distance -= Time.deltaTime * grappleRetractSpeed;
    }

    void CancelWeb()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
    }
    void CancelGrapple()
    {
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    void LookAtMouse(GameObject obj)
    {
        Vector2 direction = mousePos - (Vector2)obj.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
