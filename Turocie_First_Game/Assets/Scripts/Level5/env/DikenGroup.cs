using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DikenGroup : MonoBehaviour
{
    [Header("Settings")]
    public Vector2 DetectionSize = new Vector2(1f, 1f);
    public Vector3 PunchForce = new Vector3(0f, 0f, 0f);
    public float PunchEffDuration = 1f;
    [Range(0,10)] public int Vibaration = 5;
    [Range(0f, 10f)] public float Elasticity = 5f;



    void OnChild(GameObject child)
    {
        GameObject detectionBox = new GameObject("DetectionBox");
        detectionBox.transform.SetParent(child.transform);
        detectionBox.transform.localPosition = Vector3.zero;


        Diken dikenObj = child.GetOrAddComponent<Diken>();
        dikenObj.DetectionSize = DetectionSize;
        dikenObj.PunchForce = PunchForce;
        dikenObj.PunchEffDuration = PunchEffDuration;
        dikenObj.Vibaration = Vibaration;
        dikenObj.Elasticity = Elasticity;
        
        
    }



    // Start is called before the first frame update
    void Awake()
    {
        transform.SetAllChild(OnChild);
    }


}
