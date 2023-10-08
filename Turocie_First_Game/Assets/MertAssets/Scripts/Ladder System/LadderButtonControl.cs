using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Boy") || other.CompareTag("Doctor"))
        {
            Debug.Log("Buton Aktif");
            LadderWork.ladderIsOpen = true;
        }
    }

    
    void OnTriggerExit2D(Collider2D other)
    {
       
        if (other.CompareTag("Boy") || other.CompareTag("Doctor"))
        {
            Debug.Log("Buton Pasif");
            LadderWork.ladderIsOpen = false;
        }
    }
}
