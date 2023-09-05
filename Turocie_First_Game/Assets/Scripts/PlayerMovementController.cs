using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D boyRigidbody2D;
    [SerializeField] private LayerMask BoyCharacterLayer;

    [Header("MOVEMENT SYSTEM")]
    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 10.66f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    

   
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.W) && isGrounded())
        {
                 rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if(Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Flip();

    }

    private void FixedUpdate() 
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boy"))
        {
           Debug.Log("Doktor çocuğa çarptı!"); 
            if (boyRigidbody2D != null)
            {
                boyRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                
                
        }
    }
    }

    // private void OnCollisionStay2D(Collision2D other) 
    // {
    //     if (other.gameObject.CompareTag("Boy"))
    //     {
    //         Debug.Log("Çarpıyor"); 
    //         isHitByEnemy = false;  
    //     }
    // }

    private void OnCollisionExit2D(Collision2D other) 
    {
       if (other.gameObject.CompareTag("Boy") )
        {
           Debug.Log("Doktor çocuktan ayrıldı!"); 

            boyRigidbody2D.bodyType = RigidbodyType2D.Dynamic; 
        } 
    }

}
