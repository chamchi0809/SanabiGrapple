using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public PlayerGrappleLauncher grappleLauncher;
    
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded = false;
    private bool isMoving = false;
    private bool isApplyingDrag = false;

    public float moveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
        CheckIsGrounded();
        isMoving = CheckIsMoving();
        isApplyingDrag = CheckIsApplyingDrag();
        
        if(Input.GetButtonDown("Jump")) Jump();
    }
    
    private void FixedUpdate()
    {
        if(isMoving) ApplyMovement();
        if(isApplyingDrag) ApplyDrag();
    }

    private void ApplyMovement()
    {
        var moveDirection = new Vector2(moveInput * speed, 0);
        rb.AddForce(moveDirection);
    }
    
    private void ApplyDrag()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), .2f);
    }

    private void CheckIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    
    private bool CheckIsApplyingDrag()
    {
        if(grappleLauncher.grappleHookInstance)
        {
            if(grappleLauncher.grappleHookInstance.isArrived) return false;
        }
        
        if(!isMoving)
        {
            return true;
        }
        
        return false;
    }

    private void GetInput() => moveInput = Input.GetAxisRaw("Horizontal");

    private bool CheckIsMoving()
    {
        if(moveInput != 0)
        {
            return true;
        }

        return false;
    }
    
    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
