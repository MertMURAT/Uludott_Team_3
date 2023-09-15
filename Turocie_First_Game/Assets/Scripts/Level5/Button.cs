using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField] GameObject DetectionBox;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }



    void Detection4Button(string layerName)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(DetectionBox.transform.position, new Vector2(10f, 10f), 0f , LayerMask.GetMask("Character")); 
        foreach(var x in colls) { if (colls != null) Debug.Log(x.transform.name); }

    }


}
