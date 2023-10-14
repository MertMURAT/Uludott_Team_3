using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    
    public static event Action<GameObject> OnPlayerDeath;

    private void OnEnable()
    {
        GameOverController.OnPlayerDeath += DisableCharacterMovements;
    }

    private void OnDisable()
    {
        GameOverController.OnPlayerDeath -= DisableCharacterMovements;
    }


    public void GameOver()
    {
        OnPlayerDeath?.Invoke(this.gameObject);
    }

    void DisableCharacterMovements(GameObject gObj)
    {

        PlayerMovementController _doctorScript;
        BoyMovementController _girlScript;

        if (TryGetComponent(out _doctorScript)) _doctorScript.enabled = false;
        else if (TryGetComponent(out _girlScript)) _girlScript.enabled = false;


    }



}
