using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D doctorRigidbody2d;


    public PlayerMovementController playerMovementController;

   

     [Header("MOVEMENT SYSTEM")]
    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

     [Header("DASHİNG SYSTEM")]
     private bool canDash = true;
     private bool isDashing;
     private float dashingPower = 8f;
     private float dashingTime = 0.2f;
     private float dashingCoolDown = 1f;

     [SerializeField] private TrailRenderer tr;

    
    void Update()
    {
        if(isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.W) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if(Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();

    }

    private void FixedUpdate() 
    {
      if(isDashing)
        {
            return;
        }
    
     rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);   
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!playerMovementController.isActiveAndEnabled)
        {
           if (collision.gameObject.CompareTag("Doctor"))
        {
           //Debug.Log("Çocuk doctor' a çarptı!"); 
            if (doctorRigidbody2d != null)
            { 
                 
            }
        } 
        }

        
    }

     private void OnCollisionExit2D(Collision2D other) 
    {
       if (other.gameObject.CompareTag("Doctor"))
        {
           // Debug.Log("Çocuk doctor' dan ayrıldı!"); 
          
        } 
    }
}
