using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject LevelSelectionCanvas;
    [SerializeField] GameObject OptionsCanvas;

    public void SetLevelSelectionCanvas(bool isActive)
    {
        LevelSelectionCanvas.SetActive(isActive);
    }


    public void SetOptionsCanvas(bool isActive)
    {
        OptionsCanvas.SetActive(isActive);
    }

    public void OnLevelButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
