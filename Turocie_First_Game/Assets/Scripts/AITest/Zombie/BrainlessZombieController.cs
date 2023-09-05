using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainlessZombieController : MonoBehaviour
{


    private Rigidbody2D rbody;
    private int direction = 1;

    [SerializeField]
    public float JumpForce = 1000f;

    [SerializeField]
    public float Speed = 400f;

    [SerializeField]
    public Transform Target;



    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void MoveHorizontal()
    {
        rbody.velocity =  (new Vector2(Speed * direction * Time.deltaTime , rbody.velocity.y));
    }


    void Wander(Collider2D collision)
    {
        if ((collision.gameObject.transform.parent) &&
                    (this.transform.parent) &&
                    collision.gameObject.transform.parent.IsChildOf(this.transform.parent))
        {

            switch (collision.gameObject.name)
            {
                case "GoLeft":
                    direction = -1;
                    break;

                case "GoRight":
                    direction = 1;
                    break;

                case "JumpLeft":
                    if (direction == 1) rbody.AddForce(new Vector2(0f, JumpForce));
                    break;

                case "JumpRight":
                    if (direction == -1) rbody.AddForce(new Vector2(0f, JumpForce));
                    break;

                default: break;
            }



            //Debug.Log("Haleluya");
        }
    }


 


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (Target != null)
        {

        }

        else Wander(collision);




    }

    // Update is called once per frame
    void Update()
    {
        MoveHorizontal();
        
    }
}
