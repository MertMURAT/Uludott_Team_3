using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform playerTf;


    // Update is called once per frame
    void Update()
    {
        if(!!playerTf)transform.position = playerTf.position + new Vector3(0f , 0f , -100f);
        
    }
}
