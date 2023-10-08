using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public Vector3 Direction {get; set;}
    private GameObject itemHolding;
   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemHolding)
            {
              itemHolding.transform.position = transform.position + Direction;
              itemHolding.transform.parent = null; 
              Rigidbody2D rb = itemHolding.GetComponent<Rigidbody2D>();
              if(rb)     
                  rb.simulated = true;
                  rb.velocity = Vector2.zero;
              itemHolding = null;
            }else
            {
              Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position + Direction, .4f, pickUpMask);
            if (pickUpItem)
            {
                itemHolding = pickUpItem.gameObject;
                itemHolding.transform.position = holdSpot.position;
                itemHolding.transform.parent = transform;
                if(itemHolding.GetComponent<Rigidbody2D>())   
                   itemHolding.GetComponent<Rigidbody2D>().simulated = false;
            }
            }
        }
    }
}
