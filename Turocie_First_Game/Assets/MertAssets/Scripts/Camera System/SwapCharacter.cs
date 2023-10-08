using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacter : MonoBehaviour
{
    public PlayerMovementController playerMovementController;
    public BoyMovementController boyMovementController;
    public bool playerCharacterActive;

    void Start() {
      playerMovementController.enabled = true;
      boyMovementController.enabled = false;  
    }

    void Update() {
    if(Input.GetKeyDown(KeyCode.F))
    {
        SwitchPlayer();
    }    
    }

    public void SwitchPlayer()
    {
        if(playerCharacterActive)
        {
            playerMovementController.enabled = false;
            boyMovementController.enabled = true;
            playerCharacterActive = false;
        }else
        {
           playerMovementController.enabled = true;
           boyMovementController.enabled = false;
           playerCharacterActive = true; 
        }
    }
    
}
