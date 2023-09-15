using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class Door : MonoBehaviour , IDataPersistence
{

    [Header("Door-Key System")]
    public List<GameObject> KeyGroup;
    [SerializeField] bool _isGoalCompleted = false;
    [SerializeField] Transform _detectionBox;
    public Vector2 DetectionBoxSize = new Vector2(1f, 1f);
    public bool isInteractionAvalible = false;

    [Header("Door Settings")]
    [SerializeField] bool _isOpened = false;
    public float rotateDuration = 1f;
    public float statusDuration = 1f;
    [Range(0f,360f)] public float openAngle = 90f;
    public float gizmosRange = 10f;
     
    Transform _doorBody;
    Transform _colon;
    Collider2D _coll;
    LayerMask _characterLayer;

    [SerializeField] string _id; 

    [ContextMenu("Generate ID")]
    void GenerateID(){
        var idLength = 40;
        using(var rng = new RNGCryptoServiceProvider())
        {
            var bit_count = idLength * 6;
            var byte_count = ((bit_count + 7) / 8);
            var bytes = new byte[byte_count];
            rng.GetBytes(bytes);
            _id = System.Convert.ToBase64String(bytes);
        }
    }


    void Start()
    {
        _doorBody = transform.Find("DoorBody").transform;
        _colon = transform.Find("Colon").transform;
        _coll = _colon.GetComponent<Collider2D>();
        _detectionBox = transform.Find("DetectionBox");
        _characterLayer = LayerMask.GetMask("Character");


        if (!_isOpened) _doorBody.RotateAround(_colon.position, _colon.up , openAngle);
        if (_isGoalCompleted) KeyGroup.Clear();
    }

    public void LoadData(GameData data)
    {
        data.isDoorGoalCompleted.TryGetValue(_id, out _isGoalCompleted);
    }

    public void SaveData(GameData data)
    {
        if (data.isDoorGoalCompleted.ContainsKey(_id))
        {
            data.isDoorGoalCompleted.Remove(_id);
        }
        data.isDoorGoalCompleted.Add(_id, _isGoalCompleted);
    }


    void OpeningDoorAnimation(float duration)
    { 
            DOVirtual.Float(0f, openAngle, duration, value => {
                
                float rotationAmount = value - _doorBody.rotation.eulerAngles.y;
                _doorBody.RotateAround(_colon.position, _colon.up, rotationAmount);
            }).SetEase(Ease.InOutExpo).OnComplete(()=> { SwitchBehaviour(); });
        
    }

    void ClosingDoorAnimation(float duration)
    {
            
            DOVirtual.Float(openAngle, 0f, duration, value => {

                float rotationAmount = value - _doorBody.rotation.eulerAngles.y;
                _doorBody.RotateAround(_colon.position, _colon.up, rotationAmount);
            }).SetEase(Ease.InOutBack).OnComplete(() => { SwitchBehaviour(); });
       
    }



    public void Interaction()
    {
        if (!_isOpened)
            ClosingDoorAnimation(rotateDuration);
   
        else
            OpeningDoorAnimation(rotateDuration);   
    }

    public void SwitchBehaviour()
    {
        _isOpened = !_isOpened;
        _coll.enabled = !_isOpened;
    }

    bool Check4Keys(CharacterController character)
    {

        if (KeyGroup.Count == 0 || _isGoalCompleted) return true;
        for(int i = 0; i < KeyGroup.Count; i += 1)
            character.KeyInv.FindAndUseKey(KeyGroup[i] , (GameObject goalKey)=> {
                goalKey.GetComponent<Key>().isInInventory = false;
                KeyGroup.RemoveAt(i); 
                Destroy(goalKey); 
            });

        if(KeyGroup.Count == 0)
        {
            _isGoalCompleted = true;
            return true;
        }


        /*
        foreach(GameObject key in KeyGroup)
            if (!character.KeyInv.FindKey(key)) return false;
        _isGoalCompleted = true;
        */
        return false;
    }

    private void FixedUpdate()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(_detectionBox.position, DetectionBoxSize, 0f, _characterLayer);
        if (colls.Length == 0) isInteractionAvalible = false;
        foreach(Collider2D coll in colls)
        {
            CharacterController character = coll.GetComponent<CharacterController>();
            if (character != null && Check4Keys(character))
                isInteractionAvalible = true;
            else isInteractionAvalible = false;   
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInteractionAvalible)    Interaction();
    }


    private void OnDrawGizmos()
    {
        if(_colon != null && _doorBody != null)
        {
            Gizmos.color = Color.blue;
            Quaternion targetRotation = Quaternion.Euler(0f, openAngle, 0f);
            Vector3 offset_y = new Vector3(0f, -1.5f, 0f);

            // Calculate the pivot point for the door's rotation
            Vector3 pivotPoint = _colon.position + offset_y;

            // Calculate the door's starting and ending positions
            Vector3 startPosition = _colon.position + offset_y;
            Vector3 endPosition = pivotPoint + (targetRotation * Vector3.forward) * gizmosRange;

        
            Gizmos.DrawLine(startPosition, endPosition);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_detectionBox.position, DetectionBoxSize);


    }


}