using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GoalDoor : MonoBehaviour 
{

    public bool isGoalDoorReached = false;
    [SerializeField] LayerMask _characterLayer;
    [SerializeField] string _targetTag;
    

    [SerializeField] Transform _detectionBox;
    [SerializeField] Vector2 _detectionSize;

    [Header("Gizmos Settings")]
    public Color DetectionBoxColor;



    private void FixedUpdate()
    {
        isGoalDoorReached = false;
        this.gameObject.DetectionBoxByLayer(_detectionSize, _characterLayer, (Collider2D coll) => {

            if (coll != null && coll.gameObject.CompareTag(_targetTag))
                isGoalDoorReached = true;
             });
    }


    private void OnDrawGizmos()
    {
        if(DetectionBoxColor != null)
        {
            Gizmos.color = DetectionBoxColor;
            Gizmos.DrawWireCube(_detectionBox.position, _detectionSize);
        }
    }

}
