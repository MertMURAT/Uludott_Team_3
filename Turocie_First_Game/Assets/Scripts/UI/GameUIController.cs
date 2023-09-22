using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject LevelSelectionCanvas;
    [SerializeField] GameObject OptionsCanvas;

    [SerializeField] List<Button> MainMenuButtons;

    public void SetLevelSelectionCanvas(bool isActive)
    {
        LevelSelectionCanvas.SetActive(isActive);
    }

    public void OnClick2Reset()
    {
        DataPersistenceManager._instance.NewGame();
    }


    public void SetOptionsCanvas(bool isActive)
    {
        OptionsCanvas.SetActive(isActive);
    }

    public void OnLevelButtonClick(string levelName)
    {
        DataPersistenceManager._instance._selectedProfileID = levelName;
        DataPersistenceManager._instance.SaveGame();
        SceneManager.LoadSceneAsync(levelName);
    }



    public void DisableMainMenuButtons()
    {
        for (int i = 0; i < MainMenuButtons.Count; i += 1)
            MainMenuButtons[i].enabled = false;
            
    }


}
