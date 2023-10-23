using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("MOVEMENT SYSTEM")]
    private float horizontal;
    public float speed = 4f;
    public float jumpingPower = 10.66f;
    private bool isFacingRight = true;

    [Header("AUDIO SYSTEM")]
    [SerializeField] AudioSource _mainAudioSource;
    [SerializeField] AudioClip _walkingSound;
    [SerializeField] AudioClip _jumpingSound;
    
    
    private Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
   
    
   

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WalkingSoundCoroutine());
    }

    IEnumerator WalkingSoundCoroutine()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitUntil(() => { return (horizontal != 0 && isGrounded()); });
            if(!_mainAudioSource.isPlaying)_mainAudioSource.PlayOneShot(_walkingSound);
            
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
        Flip();

        animator.SetBool("Jump", !isGrounded());
        animator.SetBool("Walk", horizontal != 0);
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

}
