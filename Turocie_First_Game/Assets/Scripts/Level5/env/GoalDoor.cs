using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour , IDataPersistence
{

    [SerializeField] bool isGoalDoorReached = false;
    [SerializeField] List<GameObject> Characters;

    [SerializeField] Transform _detectionBox;
    [Range(0f, 50f)] Vector2 _detectionSize;


    public void LoadData(GameData data){}

    public void SaveData(GameData data)
    {
        

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
