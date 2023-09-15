using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{

    public int DeathCount;
    public SerialazibleDictionary<string , bool> isDoorGoalCompleted;
    public SerialazibleDictionary<string, bool> isKeyCollected;
    public SerialazibleDictionary<string, bool> isKeyInKeyInventory;


    public GameData()
    {
        this.DeathCount = 0;
        this.isDoorGoalCompleted = new SerialazibleDictionary<string, bool>();
        this.isKeyCollected = new SerialazibleDictionary<string, bool>();
        this.isKeyInKeyInventory = new SerialazibleDictionary<string, bool>();


        //this.isDoorOpened = new List<bool>();
    }


}
