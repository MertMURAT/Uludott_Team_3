using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField] private float detectionRange = 0.5f;
    [SerializeField] private Transform Player;

    public Transform posA, posB;
    private int patrolIndex = 0;
    IAstarAI agent;
    public bool PatrolEnabled = true;

    // For Debug Purposes
    //[SerializeField] public Vector3 targetDestination;


    // Start is called before the first frame update
    void Awake()
    {

        agent = GetComponent<IAstarAI>();
        
        
        

    }


    void PatrolPath()
    {
        // For Debug Purposes
        //targetDestination = agent.destination;

        // If there is no target destination Point , do nothing 
        if (((posA) == null) || ((posB) == null)) return;
        else if (!PatrolEnabled) return;
        bool search = false;

        // Check if the agent reached to the target AND check if the agent has reached the *previous* target
        // because the new path has not been calculated yet.
        if (agent.reachedEndOfPath && !agent.pathPending || agent.reachedDestination)
        {
            patrolIndex+=1;
            search = true;
        }

        // Go to the first target if we went to all of them 
        if ((patrolIndex % 2) == 0) agent.destination = posA.position;
        else agent.destination = posB.position;


        // Calculate a path to the target. 
        if (search) agent.SearchPath();

    }

    bool IsPlayerWithinDetectionRange() {
        return (Vector2.Distance(transform.position, Player.position) < detectionRange);
    }




    // Update is called once per frame
    void Update()
    {
        if (PatrolEnabled && !IsPlayerWithinDetectionRange()) PatrolPath(); 
        else
        {
            GetComponent<EnemyChaseAI>().enabled = true;
            GetComponent<AIPath>().enabled = false;
            this.enabled = false;

            
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        /*
        float corners = 180; // How many corners the circle should have
        float size = 10; // How wide the circle should be
        Vector3 origin = transform.position; // Where the circle will be drawn around
        Vector3 startRotation = transform.right * size; // Where the first point of the circle starts
        Vector3 lastPosition = origin + startRotation;
        float angle = 0;
        while (angle <= 360)
        {
            angle += 360 / corners;
            Vector3 nextPosition = origin + (Quaternion.Euler(0, 0, angle) * startRotation);
            Gizmos.DrawLine(lastPosition, nextPosition);
            //Gizmos.DrawSphere(nextPosition, 1);

            lastPosition = nextPosition;
        }
        */
    }

}
