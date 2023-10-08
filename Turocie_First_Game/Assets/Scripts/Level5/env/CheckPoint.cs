using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CheckPoint : MonoBehaviour , IDataPersistence
{
    [SerializeField] private string _id;
    [SerializeField] GameObject checkPointsParent;
    [SerializeField] Transform _detectionBox;
    [SerializeField] Transform _spawnLocation;
    public Vector2 DetectionBoxSize = new Vector2(1f, 1f);
    public bool isInteractionAvalible = false;
    [SerializeField] LayerMask selectedLayer;
    public Color detectionColor;
    public Color spawnLocColor;
    public bool executedOnce = false;

    float _spawnLocationDotRadius = 1f;


    [ContextMenu("Generate IDs for all checkpoints")]
    public void GenerateKeys()
    {


        if (checkPointsParent != null)
        {
            for (int i = 0; i < checkPointsParent.transform.childCount; i += 1)
            {
                string _id = Extensions.GenerateID(40);

                GameObject cpObj = checkPointsParent.transform.GetChild(i).gameObject;
                CheckPoint point = cpObj.GetOrAddComponent<CheckPoint>();

                point._id = _id;

            }

        }
        else Debug.LogError("CheckPoint parrent must be assigned!!!");


    }





    public void LoadData(GameData data)
    {
        if(!data.isLevelCompleted)
            data.isCheckPointAlreadyUsed.TryGetValue(_id, out this.executedOnce);

    }

    public void SaveData(GameData data)
    {
        if (!data.isLevelCompleted)
        {
            if(isInteractionAvalible) data.playerPosition = this._spawnLocation.position;
            if (data.isCheckPointAlreadyUsed.ContainsKey(_id)) data.isCheckPointAlreadyUsed[_id] = this.executedOnce;
            else if (_id != null && _id != "") data.isCheckPointAlreadyUsed.Add(this._id, this.executedOnce);
        }
    }


    private void FixedUpdate()
    {
        
        isInteractionAvalible = false;
        this.gameObject.DetectionBoxByLayer(DetectionBoxSize, selectedLayer, (Collider2D coll) => {

            

            if (coll != null  && !executedOnce)
            {
                isInteractionAvalible = true;
                DataPersistenceManager._instance.isCheckPointReached = true;
                executedOnce = true;
            }

            

        });
    }




    private void OnDrawGizmos()
    {
        if (detectionColor != null)
        {
            Gizmos.color = detectionColor;
            Gizmos.DrawWireCube(_detectionBox.position, DetectionBoxSize);

        }

        if(spawnLocColor != null)
        {
            Gizmos.color = spawnLocColor;
            Gizmos.DrawWireSphere(_spawnLocation.position, _spawnLocationDotRadius);
        }
            
            
    }


}
