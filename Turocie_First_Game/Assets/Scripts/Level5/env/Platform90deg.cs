using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform90deg : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform _platformBody;
    [SerializeField] Transform _rotaryJoint;
    [Range(-180f , 180f)]public float OpenAngle;
    public float RotateDuration;
    public float CooldownDuration;
    public bool rotateBackAfterCooldown = false;
    public Ease RotateUpEaseType;
    public Ease RotateBackEaseType;

    [Header("Detection Settings")]
    [SerializeField] GameObject _detectionBox;
    public Vector2 BoxSize = Vector2.one;
    public bool IsWithinDetectionRange;

    [Header("Gizmos Settings")]
    public Color GizmosRotationColor;
    public Color GizmosDetecionBoxColor;
    public float GizmosRadius = 10f;

    bool _isOnCooldown = false;
    bool _isRotateBackReady = false;
    float _startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        _startingRotation = _platformBody.eulerAngles.z % 360;
    }


    IEnumerator RotationCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        _isOnCooldown = false;
    }


    void RotateBack()
    {
        float currentRotation = _platformBody.eulerAngles.z;
        float clockwiseRotation = (360 + _startingRotation - currentRotation) % 360;
        float counterclockwiseRotation = (currentRotation - _startingRotation + 360) % 360;

        float shortestRotation = Mathf.Abs(clockwiseRotation) < Mathf.Abs(counterclockwiseRotation)
            ? clockwiseRotation
            : -counterclockwiseRotation;

        DOVirtual.Float(currentRotation, _startingRotation, RotateDuration, value => {
            float rotationAmount = value - _platformBody.eulerAngles.z;
            _platformBody.RotateAround(_rotaryJoint.position, _rotaryJoint.forward, rotationAmount);
        }).SetEase(RotateBackEaseType);
    }



    void RotateAnim()
    {
        
        float currentRotation = _platformBody.eulerAngles.z;

        
        float clockwiseRotation = (OpenAngle - currentRotation + 360) % 360;
        float counterclockwiseRotation = (currentRotation - OpenAngle + 360) % 360;

       
        float shortestRotation = Mathf.Abs(clockwiseRotation) < Mathf.Abs(counterclockwiseRotation)
            ? clockwiseRotation
            : -counterclockwiseRotation;

        
        DOVirtual.Float(currentRotation, currentRotation + shortestRotation, RotateDuration, value => {
            float rotationAmount = value - _platformBody.eulerAngles.z;
            _platformBody.RotateAround(_rotaryJoint.position, _rotaryJoint.forward, rotationAmount);
        }).SetEase(RotateUpEaseType);

        //StartCoroutine(RotationCooldown());
    }


    private void FixedUpdate()
    {
        IsWithinDetectionRange = false;
        _detectionBox.DetectionBox(BoxSize,_detectionBox.transform.eulerAngles.z , (Collider2D coll) =>
        {
            if (coll.gameObject.CompareTag("grabbable"))
            {
                IsWithinDetectionRange = true;
                RotateAnim();
                _isRotateBackReady = true;
                StartCoroutine(RotationCooldown());
            }
        });

        if ( rotateBackAfterCooldown && _isRotateBackReady && !_isOnCooldown && !IsWithinDetectionRange)
        {
            RotateBack();
            _isRotateBackReady = false;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !_isOnCooldown) RotateAnim();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmosRotationColor;

        // Display Platform rotation
        Gizmos.DrawLine(
            _rotaryJoint.position, 
            new Vector3(
                
                _rotaryJoint.position.x +  (GizmosRadius * (Mathf.Cos(OpenAngle * Mathf.Deg2Rad)) ) , 
                _rotaryJoint.position.y + (GizmosRadius * Mathf.Sin(OpenAngle * Mathf.Deg2Rad)) , 
                _rotaryJoint.position.z
                
            )
            
        );

        // Display Detection Box
        if (_detectionBox)
        {
            Gizmos.color = GizmosDetecionBoxColor;
            Gizmos.DrawWireCube(_detectionBox.transform.position, BoxSize);
        }
    }

}
