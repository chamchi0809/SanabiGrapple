using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleLauncher : MonoBehaviour
{
    public Transform grapplePoint;
    public PlayerController playerController;
    public GameObject grappleHookPrefab;
    public GrappleHook grappleHookInstance;
    private Vector2 mouseDirection;
    public DistanceJoint2D joint;
    
    [Header("Dash")]
    public float dashDuration;
    public float dashSpeed;
    private float dashStartInput;
    private float dashTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - grapplePoint.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            LaunchGrapple();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnCancelGrapple();
        }
        
        joint.enabled = grappleHookInstance && grappleHookInstance.isArrived;
        if (grappleHookInstance && grappleHookInstance.isArrived)
        {
            joint.connectedBody = grappleHookInstance.GetComponent<Rigidbody2D>();
            joint.connectedAnchor = Vector2.zero;
            joint.anchor = grapplePoint.localPosition;
            joint.distance = Vector2.Distance(grapplePoint.position, grappleHookInstance.transform.position);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash();
        }
    }
    
    private void LaunchGrapple()
    {
        if(grappleHookInstance != null)
            Destroy(grappleHookInstance.gameObject);
        GameObject grappleHook = Instantiate(grappleHookPrefab, grapplePoint.position, Quaternion.identity);
        grappleHookInstance = grappleHook.GetComponent<GrappleHook>();
        grappleHookInstance.LaunchGrapple(mouseDirection.normalized, grapplePoint);
    }
    
    private void OnCancelGrapple()
    {
        if(grappleHookInstance != null)
            Destroy(grappleHookInstance.gameObject);
    }

    private void OnDash()
    {
        if (Time.time - dashTime < dashDuration) return;
        dashTime = Time.time;
        dashStartInput = Math.Sign(playerController.moveInput);
        //make perpendicular to direction to grappler
        var toGrappler = (grappleHookInstance.transform.position - transform.position).normalized;
        //make it clockwise
        var dashDirectionClockwise = new Vector2(toGrappler.y, -toGrappler.x);
        //make it counter clockwise
        var dashDirectionCounterClockwise = new Vector2(-toGrappler.y, toGrappler.x);
        var dashDirection = dashStartInput > 0 ? dashDirectionClockwise : dashDirectionCounterClockwise;
        playerController.rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(grapplePoint.position, grapplePoint.position + (Vector3)mouseDirection.normalized);
    }
}
