using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RollingStone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform pathParent;
    public float PathDuration = 2f;
    public PathType TypeOfPath;
    public float RotateDurationPerTurn = 3f;
    public bool isActivated = false;
    public Ease EaseType;

    bool _rollOnlyOnce = true;

    [Header("DetectionCircle Settings")]
    [SerializeField] Transform _detectionCircle;
    public Color GizmosColor = Color.clear;
    public float DetectionRadius = 3f;



    // Start is called before the first frame update
    void Start()
    {
        if (!_detectionCircle) _detectionCircle = transform.GetChildByName("DetectionCircle").transform;
    }

    void StartRolling()
    {
        if (_rollOnlyOnce && isActivated) {
            Rolling();
            _rollOnlyOnce = false;
        }
        
    }

    void Rolling()
    {

        Vector3[] pathArray = new Vector3[pathParent.childCount];
        for (int i = 0; i < pathArray.Length; i += 1)
            pathArray[i] = pathParent.GetChild(i).position;


        transform.DORotate(new Vector3(0f, 0f, -180f), RotateDurationPerTurn).SetLoops(-1, LoopType.Incremental);
        transform.DOPath(pathArray, PathDuration, TypeOfPath, PathMode.TopDown2D, 10, Color.white)
            .OnComplete(() => Destroy(gameObject))
            .SetEase(EaseType)
        ;
        
    }

    void DetectionCircleByLayer(string layerName , System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(_detectionCircle.position, DetectionRadius, LayerMask.NameToLayer(layerName));
        foreach (Collider2D coll in colls)  OnDetection(coll);
    }

    private void FixedUpdate()
    {
        DetectionCircleByLayer("Enemies", (Collider2D coll) => {

            ZombieController zombie = coll.GetComponent<ZombieController>();
            if (zombie)
                zombie.OnChrush();
        });
    }


    // Update is called once per frame
    void Update()
    {
        StartRolling();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawWireSphere(_detectionCircle.position, DetectionRadius);
    }



}
