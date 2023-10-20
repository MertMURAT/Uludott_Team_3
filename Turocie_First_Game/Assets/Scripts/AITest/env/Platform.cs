using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Platform : MonoBehaviour
{

    [SerializeField]
    public Transform posA, posB;



    [Header("Settings")]
    public float Time = 0;
    public Vector2 d = Vector2.zero;
    public float ForceConstant;
    public Vector2 direction = Vector2.one;
    public ForceMode2D fmode;
    public bool ApplyRelativeForceOnYAxis = false;
    





    Vector3 targetPos;
    Rigidbody2D rb;
    float _angle;


    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        d = new Vector2(Mathf.Abs(posA.position.x - posB.position.x), Mathf.Abs(posA.position.y - posB.position.y));
        _angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

        
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Vector2.Distance(transform.position, posA.position) < 0.1f) targetPos = posB.position;
        if (Vector2.Distance(transform.position, posB.position) < 0.1f) targetPos = posA.position;


        //transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
        

    }

    private void FixedUpdate()
    {
        direction.x = (targetPos.x < transform.position.x) ? -1 : 1;
        direction.y = (targetPos.y < transform.position.y) ? -1 : 1;
        rb.velocity = direction * new Vector2(d.x / Time, d.y / Time);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            if (ApplyRelativeForceOnYAxis)
                collision.attachedRigidbody.AddForce(ForceConstant*(1/Time) * direction * new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)), fmode);
            else 
                collision.attachedRigidbody.AddForce(ForceConstant *(1/Time) * direction * new Vector2(Mathf.Cos(_angle), 0f ), fmode);
        }
    }


}
