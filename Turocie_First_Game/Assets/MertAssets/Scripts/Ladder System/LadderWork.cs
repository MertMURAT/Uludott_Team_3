using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderWork : MonoBehaviour
{
    private Animator animator;
    public static bool ladderIsOpen;
    // Start is called before the first frame update
    void Start()
    {
         animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.E) && ladderIsOpen)
        {
            Debug.Log("Merdiven açılma animasyonu çalıştı");
            animator.SetTrigger("LadderWork");
        }  
    }

}
