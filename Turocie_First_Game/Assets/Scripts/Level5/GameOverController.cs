using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    
    public static event Action<GameObject> OnPlayerDeath;
    [SerializeField] GameObject _playerParent;
    [SerializeField] GameObject _audioSourceObj;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] bool isDead = false;

    private void OnEnable()
    {
        GameOverController.OnPlayerDeath += DisableCharacterMovements;
        GameOverController.OnPlayerDeath += PlayCharacterDeathSound;
    }

    private void OnDisable()
    {
        GameOverController.OnPlayerDeath -= DisableCharacterMovements;
        GameOverController.OnPlayerDeath -= PlayCharacterDeathSound;
        isDead = false;
    }


    public void GameOver()
    {
        if(!isDead) OnPlayerDeath?.Invoke(this.gameObject);
        isDead = true;
    }

    void DisableCharacterMovements(GameObject gObj)
    {
        BoyMovementController l_girlScript;
        PlayerMovementController l_doctorScript;

        for(int i = 0; i < _playerParent.transform.childCount; i += 1)
        {
            Transform player = _playerParent.transform.GetChild(i);
            if (player.TryGetComponent(out l_doctorScript)) l_doctorScript.enabled = false;
            else if (player.TryGetComponent(out l_girlScript)) l_girlScript.enabled = false;

        }


    }

    void PlayCharacterDeathSound(GameObject dyingPlayer)
    {
        AudioSource a_src = _audioSourceObj.GetComponent<AudioSource>();
        if(dyingPlayer.Equals(_audioSourceObj.transform.parent.gameObject))
            a_src.PlayOneShot(_deathSound);
    }



}
