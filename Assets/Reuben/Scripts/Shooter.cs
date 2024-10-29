using System;
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
    [SerializeField] private GameObject grappleHook;
    [SerializeField] private GameObject grappleRangePoint;

    bool grappling = false;
    bool shooting = false;
    public float webRetractSpeed;
    [SerializeField] float grappleRetractForce;
    [SerializeField] float grappleSpeed;
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

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 grapplePos = new Vector2(grappleHook.transform.position.x, grappleHook.transform.position.y);

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
            FireShot(ShotType.Web, direction, grapplePos);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            shooting = false;
            CancelShot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            grappling = true;
            shooting = true;
            FireShot(ShotType.Grapple, direction, grapplePos);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            grappling = false;
            shooting = false;
            StopAllCoroutines();
            StartCoroutine(CancelGrapple());
        }
        if (grappling)
        {
            lineRenderer.SetPosition(0, grappleHook.transform.position);
            lineRenderer.SetPosition(1, shooter.transform.position);
        }

        if (distanceJoint.enabled || springJoint.enabled)
        {
            Contract();
            lineRenderer.SetPosition(1, shooter.transform.position);
        }
    }

    IEnumerator FireGrapple(Vector2 direction, Vector2 grapplePos)
    {
        if (CastRay())
        {
            while (grapplePos != shooterHit.point)
            {
                LookAt(shooterHit.point);
                grappleHook.transform.position = Vector3.MoveTowards(grappleHook.transform.position, shooterHit.point, grappleSpeed * Time.deltaTime);
                grapplePos = new Vector2(grappleHook.transform.position.x, grappleHook.transform.position.y);
                yield return null;
            }
            ShootPlayerAtGrapple(direction);
            StopCoroutine(FireGrapple(direction, grapplePos));
        }
    }

    void ShootPlayerAtGrapple(Vector2 direction)
    {
        StartCoroutine(CancelGrapple());
        rb.AddForce(direction * grappleRetractForce, ForceMode2D.Impulse);
    }

    IEnumerator CancelGrapple()
    {
        while (grappleHook.transform.position != shooter.transform.position)
        {
            grappleHook.transform.position = Vector3.Lerp(grappleHook.transform.position, shooter.transform.position, grappleSpeed);
            yield return null;
        }
        if (grappleHook.transform.position == shooter.transform.position)
        {
            lineRenderer.enabled = false;
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

    void FireShot(ShotType type, Vector2 direction, Vector2 grapplePos)
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

                StartCoroutine(FireGrapple(direction, grapplePos));
                lineRenderer.enabled = true;
                break;
        }
    }

    void Contract()
    {
        //springJoint (web)
        float distance = springJoint.distance;
        springJoint.distance = Mathf.Lerp(distance, 0, distance * webRetractSpeed * Time.deltaTime);
    }

    void CancelShot()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        distanceJoint.enabled = false;
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
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation, Time.deltaTime * angleResetSpeed);
            yield return null;
        }

        obj.transform.rotation = targetRotation;
    }



}
