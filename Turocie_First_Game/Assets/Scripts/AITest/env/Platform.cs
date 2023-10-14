using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public LayerMask layer ;
    [SerializeField]
    public Transform posA, posB;

    [SerializeField]
    public float Speed;

    [Header("DetectionBox Settings")]
    [SerializeField] GameObject _detectionBox;
    [SerializeField] Vector3 _detectionBoxSize;
    public bool isInteractionAvalible = false;


    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, posA.position) < 0.1f) targetPos = posB.position;
        if (Vector2.Distance(transform.position, posB.position) < 0.1f) targetPos = posA.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos , Speed ); 

    }

    private void FixedUpdate()
    {
        this.gameObject.DetectionBoxByLayer(_detectionBoxSize,layer,(Collider2D coll)=>{
            coll.transform.parent = this.transform ;
        });

            
        
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