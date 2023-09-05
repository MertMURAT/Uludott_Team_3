using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private Rigidbody2D rbody;

    [SerializeField]
    public float walkingSpeed = 150f;

    [SerializeField]
    public float jumpPower = 500f;




    // Start is called before the first frame update
    void Awake()    {

        rbody = GetComponent<Rigidbody2D>();

        

    }
    void PlayerMovement()
    {
        // Horizontal Movement
        rbody.velocity = (new Vector2(Input.GetAxisRaw("Horizontal") * walkingSpeed * Time.deltaTime, rbody.velocity.y));

        // Verticle Movement : Jump 
        if (Input.GetButtonDown("Jump"))
        {
            rbody.AddForce(new Vector2(0f, jumpPower * Time.deltaTime), ForceMode2D.Impulse);
        }
    }

    // Update is called o

        
    void Update()
    {
        PlayerMovement();
        
    }
}
