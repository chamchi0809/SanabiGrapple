using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LayerMask grappleableMask;
    public Transform grapplePoint;
    public float speed = 5f;
    public float grappleDistance = 5f;
    public bool isArrived = false;
    
    private Vector2 lanchDirection;
    private bool isLanched = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        lineRenderer.enabled = isLanched || isArrived;
        
        if(Vector2.Distance(transform.position, grapplePoint.position) > grappleDistance && isLanched)
        {
            isLanched = false;
        }
        
        if(isLanched || isArrived)
        {
            lineRenderer.SetPosition(0, grapplePoint.position);
            lineRenderer.SetPosition(1, transform.position);
        }
        
        CheckIsHit();
    }
    
    private void FixedUpdate()
    {
        if (isLanched)
        {
            transform.position += (Vector3)lanchDirection * speed * Time.fixedDeltaTime;
        }
    }

    private void CheckIsHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lanchDirection, .1f, grappleableMask);
        if (hit.collider != null)
        {

            if (!isArrived)
            {
                OnArrived();
            }
            isArrived = true;
        }
    }
    
    private void OnArrived()
    {
        Debug.Log("hit");
        isLanched = false;
        isArrived = false;
    }
    
    public void LaunchGrapple(Vector2 direction, Transform grapplePoint)
    {
        isLanched = true;
        lanchDirection = direction;
        this.grapplePoint = grapplePoint;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}
