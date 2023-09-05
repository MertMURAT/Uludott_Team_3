using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaseAI : MonoBehaviour
{

    [Header("Pathfinding")]
    public Transform target;
    public float activeDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.1f;
    public float jumpCheckOffset = 0.5f;

    private Vector2 currentVelocity;


    [Header("Custom Behaviour")]
    public bool followEnabled = false;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    bool isGrounded = false;
    private int currentWayPoint = 0;
    private Path path;
    Seeker seeker;
    Rigidbody2D rb;
    Collider2D col;
    LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {

        

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        groundLayer = LayerMask.GetMask("Ground");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if((followEnabled) && (TargetInDistance()))
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if ((followEnabled) && (TargetInDistance()) && seeker.IsDone()) 
            seeker.StartPath(rb.position, target.position, OnPathComplete);

    }

    private void PathFollow()
    {
        // If there is no path or All the paths are traversed , stop following.
        if (path == null) return;
        if (currentWayPoint >= path.vectorPath.Count) return;


        // check if the enemy is grounded before jumping
        RaycastHit2D isGrounded = Physics2D.BoxCast(
                col.bounds.center,
                col.bounds.size,
                0,
                Vector2.down,
                0.1f,
                groundLayer
        );


        // Calculate direction for Jump and Movement.
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        if (!isGrounded) force.y = 0; // in order to prevent jlittering

        // Jump Mechanic
        if (jumpEnabled && isGrounded)
            if (direction.y > jumpNodeHeightRequirement)
                rb.AddForce(Vector2.up * speed * jumpModifier);

        // Movement
     // Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);

        // Pass to the next Waypoint if the arriving conditions is provided
        float distance = Vector2.Distance(rb.position , path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance) currentWayPoint++;

        // Flip
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (rb.velocity.x < -0.05f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
            
    }

    private bool TargetInDistance() { return Vector2.Distance(transform.position, target.position) < activeDistance; }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    
}
