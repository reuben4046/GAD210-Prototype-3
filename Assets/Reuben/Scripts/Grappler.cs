using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{

    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public SpringJoint2D springJoint;

    public float retractSpeed = 0.5f;

    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject webShooter;

    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        springJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.SetPosition(0, mousePos);
            lineRenderer.SetPosition(1, transform.position);
            springJoint.connectedAnchor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            springJoint.enabled = true;
            lineRenderer.enabled = true;
        }
        if (Input.GetMouseButton(0))
        {
            springJoint.distance -= Time.deltaTime * retractSpeed;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            springJoint.enabled = false;
            lineRenderer.enabled = false;
        }
        if (springJoint.enabled)
        {
            lineRenderer.SetPosition(1, webShooter.transform.position);
        }

        PivotLookAtMouse();
    }

    void PivotLookAtMouse()
    {
        Vector2 direction = mousePos - (Vector2)pivot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
