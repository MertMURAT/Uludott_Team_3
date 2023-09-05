using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalLocationSystem : MonoBehaviour
{

    [SerializeField] GameObject[] GoalPoints;
    [SerializeField] GameObject Arrow;
    

    Transform _currentGoalPoint;
    float _angle = 0;


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    void AdjustArrow()
    {
        Vector3 diff = _currentGoalPoint.position - transform.position;
        _angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Arrow.transform.rotation = Quaternion.Euler(0, 0, _angle - 90);
        
    }

    private void FixedUpdate()
    {
        _currentGoalPoint = GoalPoints[0].transform;
        AdjustArrow();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
