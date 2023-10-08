using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour , IDataPersistence
{

    [SerializeField] List<GoalDoor> GoalDoors;
    [SerializeField] bool isLevelCompleted;



    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(GameData data)
    {
        data.isLevelCompleted = this.isLevelCompleted;
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
            DataPersistenceManager._instance.SaveGame();

        }
            
            
            
    }
}
