using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{

    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;
    
    
   
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private Rigidbody2D rb2;
 
 


    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        if (isLadder && Mathf.Abs(vertical) > 0f || Mathf.Abs(vertical) < 0f)
        {
            isClimbing = true;
        }

         
    }

    void FixedUpdate()
    {
      if (isClimbing)
      {
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
      }  
      else
      {
        rb.gravityScale = 4f;
      }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        
        }
    }
}
