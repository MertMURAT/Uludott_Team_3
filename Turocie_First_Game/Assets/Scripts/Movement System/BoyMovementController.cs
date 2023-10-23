using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMovementController : MonoBehaviour
{

     [Header("MOVEMENT SYSTEM")]
    private float horizontal;
    public float speed = 5f;
    public float jumpingPower = 10f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    [Header("WALL SLIDING SYSTEM")]
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);


    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    

     [Header("DASHİNG SYSTEM")]
     private bool canDash = true;
     private bool isDashing;
     private float dashingPower = 100f;
     private float dashingTime = 0.1f;
     private float dashingCoolDown = 1f;

     [SerializeField] private TrailRenderer tr;

    [Header("AUDIO SYSTEM")]
    [SerializeField] AudioSource _mainAudioSource;
    [SerializeField] AudioClip _walkingSound;
    [SerializeField] AudioClip _jumpingSound;


    private Animator animator;

   
   void Start()
   {
       animator = GetComponent<Animator>();
       StartCoroutine(WalkingSoundCoroutine());
   }

    private void OnDisable()
    {
        StopAllCoroutines();
    }



    IEnumerator WalkingSoundCoroutine()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitUntil(() => { return (horizontal != 0 && isGrounded()); });
            if (!_mainAudioSource.isPlaying) _mainAudioSource.PlayOneShot(_walkingSound);

        }
    }

    void PlayJumpingSound()
    {
        if (_mainAudioSource.isPlaying)
        {
            _mainAudioSource.Stop();
            _mainAudioSource.PlayOneShot(_jumpingSound);
        }
        else _mainAudioSource.PlayOneShot(_jumpingSound);
    }

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
            PlayJumpingSound();
            
        }
        if(Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        wallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        animator.SetBool("Jump", !isGrounded());
        animator.SetBool("Walk", horizontal != 0);

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     // F tuşuna basıldığında Push animasyonunu başlat
        //     animator.SetBool("Push", true);
        // }

    }

    // private void LateUpdate()
    // {
    //     if (Input.GetKeyUp(KeyCode.R))
    //     {
    //         animator.SetBool("Push", false);
    //     }
    // }

    private void FixedUpdate() 
    {
      if(isDashing)
        {
            return;
        }
    if (!isWallJumping)
    {
       rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);    
    }
     
    }

     //Yer temas kontrol
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //Duvar temas kontrol
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    private void wallSlide()
    {
        if (isWalled() && !isGrounded() && horizontal != 0f )
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0F)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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

    // private void OnTriggerStay2D(Collider2D other)
    // {
        
    //         if (Input.GetKey(KeyCode.R))
    //         {
    //         // Objeyi it
    //         // Örneğin, objenin sağa doğru hareket etmesini sağlayabiliriz
    //         rb.AddForce(new Vector2(10f, 0f), ForceMode2D.Impulse);
    //         }
        
    // }


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

}
