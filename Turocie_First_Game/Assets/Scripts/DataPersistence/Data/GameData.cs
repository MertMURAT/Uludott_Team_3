using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{

    public int DeathCount;
    public bool isLevelCompleted;
    public SerialazibleDictionary<string, bool> isDoorPlusGoalCompleted;
    public SerialazibleDictionary<string, int> keyPlusStates;


    public Vector3 playerPosition;




    public GameData()
    {
        this.DeathCount = 0;
        this.isLevelCompleted = false;
        this.isDoorPlusGoalCompleted = new SerialazibleDictionary<string, bool>();
        this.keyPlusStates = new SerialazibleDictionary<string, int>();
        this.playerPosition = Vector3.zero;



    }



}
