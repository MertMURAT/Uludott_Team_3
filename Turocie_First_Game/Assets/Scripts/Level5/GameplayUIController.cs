using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUIController : MonoBehaviour
{

    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Selectable[] _buttons;


    private void OnEnable()
    {
        GameOverController.OnPlayerDeath += EnableGameOverPanel;
    }


    private void OnDisable()
    {
        GameOverController.OnPlayerDeath -= EnableGameOverPanel;
    }


    void EnableGameOverPanel(GameObject _)
    {
        _gameOverPanel.SetActive(true); 
        foreach( Selectable button in _buttons) button.interactable = true;
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnReturn2MainMenuClicked()
    {
        DataPersistenceManager._instance._selectedProfileID = null;
        SceneManager.LoadScene(0);
    }

    void DisableGameOverPanel(GameObject _)
    {
        foreach (Selectable button in _buttons) button.interactable = false;
        _gameOverPanel.SetActive(false);
    }


}
