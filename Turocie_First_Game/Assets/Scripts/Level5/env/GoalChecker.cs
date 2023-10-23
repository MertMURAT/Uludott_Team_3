using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalChecker : MonoBehaviour , IDataPersistence
{

    [SerializeField] List<GoalDoor> GoalDoors;
    [SerializeField] bool isLevelCompleted;
    [SerializeField] static int maxLevel = 10;



    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(GameData data)
    {
        data.isLevelCompleted = this.isLevelCompleted;
    }

    public void GoToNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if(currentLevel < maxLevel)
        {
            int nextLevelIndex = currentLevel + 1;
            DataPersistenceManager._instance._selectedProfileID = "Bölüm" + nextLevelIndex.ToString();
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            DataPersistenceManager._instance._selectedProfileID = null;
            SceneManager.LoadScene(0);
        }
    }


    private void FixedUpdate()
    {
        
        foreach (GoalDoor door in GoalDoors)
            if (!door.isGoalDoorReached)
            {
                isLevelCompleted = false;
                return;
            }
        
        if (!this.isLevelCompleted)
        {
            this.isLevelCompleted = true;
            if(DataPersistenceManager._instance) DataPersistenceManager._instance.SaveGame();
            else
            {
                Debug.LogWarning("DataPersistenceManager Object couldnt found. Returning to mainMenu");
                SceneManager.LoadScene(0);
                return;

            }
            GoToNextLevel();
        }
    
    }
}
