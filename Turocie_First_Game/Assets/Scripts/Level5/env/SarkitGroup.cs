using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarkitGroup : MonoBehaviour
{

    [Header("Settings")]
    public float distanceY = 13.7f;
    public float distanceX = 0f;
    public float roadDuration = 4f;
    public float vibarationDurion = 4f;


    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++) {

            GameObject child = transform.GetChild(i).gameObject;
            
            
            var trigger = child.AddComponent<PolygonCollider2D>();
            trigger.isTrigger = true;

            Sarkit sarkitObj = child.GetOrAddComponent<Sarkit>();
            if (!!sarkitObj)
            {
                sarkitObj.DistanceX = distanceX;
                sarkitObj.DistanceY = distanceY;
                sarkitObj.RoadDuration = roadDuration;
                sarkitObj.VibarationDuration = vibarationDurion;
            }

            
        }                       
    }





}
