using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    public Transform posA, posB;

    [SerializeField]
    public int Speed;

    [Header("DetectionBox Settings")]
    [SerializeField] GameObject _detectionBox;
    [SerializeField] Vector3 _detectionBoxSize;
    public bool isInteractionAvalible = false;


    Vector3 targetPos;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
        TryGetComponent(out rb);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        if (Vector2.Distance(transform.position, posA.position) < 0.1f) targetPos = posB.position;
        if (Vector2.Distance(transform.position, posB.position) < 0.1f) targetPos = posA.position;

        if (this.rb) 
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, Speed * Time.deltaTime));

         
        
    }

    private void FixedUpdate()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            _detectionBox.transform.position, 
            _detectionBoxSize , 
            _detectionBox.transform.localEulerAngles.z
        );

        isInteractionAvalible = false;
        foreach(Collider2D coll in colls)
        {
            if(coll != null)
            {
                isInteractionAvalible = true;
                Rigidbody2D rb;
                if(coll.TryGetComponent(out rb))
                {
                    var direction = Vector3.zero;
                    if (Vector3.Distance(transform.position, targetPos) > 1f)
                    {
                        direction = targetPos - transform.position;
                        rb.AddRelativeForce(direction.normalized * Speed, ForceMode2D.Force);
                    }

                }
            }
        }
    }

    private void OnDrawGizmos()
    {

        if(_detectionBox != null && _detectionBoxSize != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(_detectionBox.transform.position, _detectionBoxSize);

        }
    }

}
