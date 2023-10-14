using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offSet = new Vector3(0f, 1f, -10f);
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform targetBoy;
    [SerializeField] private Transform targetDoctor;

    public BoyMovementController boyMovementController;
    public PlayerMovementController playerMovementController;

    void Update()
    {

        if(playerMovementController.isActiveAndEnabled)
        {
            Vector3 targetPosition = targetDoctor.position + offSet;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }else if(boyMovementController.isActiveAndEnabled)
        {
            Vector3 targetPosition = targetBoy.position + offSet;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
       
    }



}
