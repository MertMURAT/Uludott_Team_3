using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGridController : MonoBehaviour
{

    int completedLevels = 0;

    // Start is called before the first frame update
    private void OnEnable()
    {

        Dictionary<string, GameData> levelData = DataPersistenceManager._instance.GetAllProfilesGameData();

        for (int i = 0; i < gameObject.transform.childCount; i += 1)
        {
            gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = ("Level " + (i+1));

            //if (!levelData["Level" + (i + 1)].isLevelCompleted) completedLevels++;//Destroy(gameObject.transform.GetChild(i).GetComponent<Button>());
            
        }

        foreach (KeyValuePair<string, GameData> pair in levelData) if (pair.Value.isLevelCompleted) completedLevels++;

        for (int i = completedLevels; i <gameObject.transform.childCount -1; i += 1) 
            gameObject.transform.GetChild(i+1).GetComponent<Selectable>().interactable = false;


    }
}
