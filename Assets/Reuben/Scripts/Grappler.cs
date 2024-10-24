using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;

    [SerializeField] private GameObject webShooterPivot;
    [SerializeField] private GameObject webShooter;

    public float retractSpeed;

    [SerializeField] private float webRange = 10f;

    RaycastHit2D webHit;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        LookAtMouse(webShooterPivot);

        if (Input.GetMouseButtonDown(0))
        {
            if (CastGrappleRay())
            {
                FireWeb();
            }
        }
        if (Input.GetMouseButton(0))
        {
            ContractWeb();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            CancelWeb();
        }
        if (springJoint.enabled)
        {
            lineRenderer.SetPosition(1, webShooter.transform.position);
        }
    }

    bool CastGrappleRay()
    {
        Vector2 castDirection = (mousePos - (Vector2)webShooter.transform.position).normalized;
        webHit = Physics2D.Raycast(webShooter.transform.position, castDirection, webRange);
        if (webHit && webHit.collider.gameObject.layer != 6)
        {
            Debug.Log(webHit.collider.name);
            return true;
        }
        Debug.Log("No hit");
        return false;
    }

    void FireWeb()
    {
        lineRenderer.SetPosition(0, webHit.point);
        lineRenderer.SetPosition(1, transform.position);
        springJoint.connectedAnchor = webHit.point;
        springJoint.enabled = true;
        lineRenderer.enabled = true;
    }
    void ContractWeb()
    {
        //springJoint.distance -= Time.deltaTime * retractSpeed;
        float distance = springJoint.distance;
        springJoint.distance = Mathf.Lerp(distance, 0, distance * retractSpeed * Time.deltaTime);
    }
    void CancelWeb()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    void LookAtMouse(GameObject obj)
    {
        Vector2 direction = mousePos - (Vector2)obj.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
