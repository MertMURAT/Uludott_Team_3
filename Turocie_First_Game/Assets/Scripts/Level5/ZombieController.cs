using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ZombieController : MonoBehaviour
{

    [Header("Patrol Settings")]
    public GameObject PathParent;
    public float RoadCompletionThreshold = 0.1f;
    public float Speed = 1f;
    

    [Header("Chrush Settings")]
    public bool IsChrushable = false;
    public bool IsChrushed = false;
    public bool IsOnCooldown = false;
    public float ChrushCooldown = 4f;

    [Header("Scaling-Down Settings")]
    [Range(0.01f, 1f)] public float ScalingDownFactor = 0.25f;
    public float ScalingDownDuration = 1f;
    public Ease ScalingDownEaseType;

    [Header("Scaling-Up Settings")]
    [Range(1f , 100f)]public float ScalingUpFactor;
    public float ScalingUpDuration = 0.3f;
    public Ease ScalingUpEaseType;
    
    Rigidbody2D rbdy;
    Sequence chrushAnim;
    Vector3 _scaleXYZ0;

    // Private Patrol Fields
    struct PatrolUtilities
    {
        public Vector2[] pathPoints;
        public int currentPoint;
        

        public PatrolUtilities(GameObject PathParent)
        {
            
            pathPoints = new Vector2[PathParent.transform.childCount];
            for (int i = 0; i < PathParent.transform.childCount; i += 1) pathPoints[i] = PathParent.transform.GetChild(i).transform.position;
            currentPoint = 0;
        }
    }

    private PatrolUtilities pu;

    
    void Start()
    {
        _scaleXYZ0 = transform.localScale;
        rbdy = transform.GetComponent<Rigidbody2D>();
        pu = new PatrolUtilities(PathParent);
    }

    void PatrolMovement()
    {
        try
        {
            if (Vector2.Distance(rbdy.position, pu.pathPoints[pu.currentPoint]) < RoadCompletionThreshold) 
                pu.currentPoint = ((pu.currentPoint + 1)%(pu.pathPoints.Length));

            rbdy.position = Vector2.MoveTowards(rbdy.position, pu.pathPoints[pu.currentPoint] , Speed * Time.deltaTime);
        }
        catch(System.Exception e)
        {
            Debug.Log("currentPoint:" + pu.currentPoint + "\t pathPoints'  Length: " + +pu.pathPoints.Length + "\n " + e.Message);
        }
        
        
    }

    private void FixedUpdate()
    {
        if (!IsChrushed) PatrolMovement(); 
    }


    private void Chrush()
    {
        this.IsChrushed = IsChrushable;
        if(IsChrushable)
        {
            chrushAnim = DOTween.Sequence();
            chrushAnim.Append(transform.DOScaleY(transform.localScale.y * ScalingDownFactor, ScalingDownDuration).SetEase(ScalingDownEaseType));
            chrushAnim.Append(transform.DOScale(_scaleXYZ0, ScalingUpDuration).SetEase(ScalingUpEaseType).OnComplete(()=> { this.IsChrushed = false; }));
        }
    }

    public void OnChrush() {

        if (!IsOnCooldown)
        {
            Chrush();
            StartCoroutine(OnChrushCooldown());
        }

    }

    private IEnumerator OnChrushCooldown()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(ChrushCooldown);
        IsOnCooldown = false;
    }
}
