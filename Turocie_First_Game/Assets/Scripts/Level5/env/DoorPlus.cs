using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Events;


[System.Serializable]
public class DoorPlus : MonoBehaviour , IDataPersistence
{
    [Header("Key Settings")]
    public GameObject KeyGroup;
    public GameObject KeyInventory;
    public SerialazibleDictionary<string , bool> IDList;


    [Header("DoorEvent")]
    public UnityEvent OnInteraction;


    [Header("Door Settings")]
    [SerializeField]Transform _doorBody;
    [SerializeField]Transform _colon;
    [SerializeField] bool _isOpened = false;
    public float rotateDuration = 1f;
    public float statusDuration = 1f;
    [Range(-360f, 360f)] public float openAngle = 90f;
    public float gizmosRange = 10f;
    [SerializeField] Transform _detectionBox;
    public Vector2 DetectionBoxSize = new Vector2(1f, 1f);
    public bool isInteractionAvalible = false;
    public bool isLocked = true;
    public bool isSwitchOnCooldown = false;
    public bool isOpeningAllowed = true;
    public bool isClosingAllowed = true;
    public float SwitchCooldownDuration = 1f;



    Collider2D _coll;
    LayerMask _characterLayer;

    [ContextMenu("Generate Keys")]
    public void GenerateKeys()
    {
        if (IDList == null) IDList = new SerialazibleDictionary<string, bool>();
        else IDList.Clear(); 

        for(int i = 0; i < KeyGroup.transform.childCount; i += 1)
        {
            string _id = Extensions.GenerateID(40);
            IDList.Add(_id , false);



            GameObject keyObj = KeyGroup.transform.GetChild(i).gameObject;
            KeyPlus key = keyObj.GetOrAddComponent<KeyPlus>();
            key.keyInv = KeyInventory;
            key.ID = _id;
            keyObj.GetOrAddComponent<CircleCollider2D>().isTrigger = true;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _coll = _colon.GetComponent<Collider2D>();
        _characterLayer = LayerMask.GetMask("Character");

        
        if (!_isOpened) _doorBody.RotateAround(_colon.position, _colon.up, openAngle);
    }

    void OpeningDoorAnimation(float duration)
    {
        DOVirtual.Float(0f, openAngle, duration, value => {

            float rotationAmount = value - _doorBody.rotation.eulerAngles.y;
            _doorBody.RotateAround(_colon.position, _colon.up, rotationAmount);
        }).SetEase(Ease.InOutExpo).OnComplete(() => { SwitchBehaviour(); });

    }

    void ClosingDoorAnimation(float duration)
    {

        DOVirtual.Float(openAngle, 0f, duration, value => {

            float rotationAmount = value - _doorBody.rotation.eulerAngles.y;
            _doorBody.RotateAround(_colon.position, _colon.up, rotationAmount);
        }).SetEase(Ease.InOutBack).OnComplete(() => { SwitchBehaviour(); });

    }


    public void SwitchBehaviour()
    {
        if (!isOpeningAllowed || !isClosingAllowed) _isOpened = !isClosingAllowed;
        else _isOpened = !_isOpened;

        if (OnInteraction != null) OnInteraction.Invoke();


        
    }


    public void SwitchColonCollider()
    {
        if (_colon.TryGetComponent(out Collider2D collider)) collider.enabled = !_isOpened;
    }

    public IEnumerator DoorInteractionCooldown()
    {
        isSwitchOnCooldown = true;
        yield return new WaitForSeconds(SwitchCooldownDuration);
        isSwitchOnCooldown = false;
    }


    private void FixedUpdate()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(_detectionBox.position, DetectionBoxSize, 0f, _characterLayer);
        if (colls.Length == 0) isInteractionAvalible = false;
        foreach (Collider2D coll in colls)
        {   
                isInteractionAvalible = true;   
        }

       

    }

    void Try2UnlockDoor()
    {
        for (int i = 0; i < KeyInventory.transform.childCount; i += 1)
        {
                KeyPlus key = KeyInventory.transform.GetChild(i).GetComponent<KeyPlus>();
                if (IDList.ContainsKey(key.ID))
                {
                    IDList[key.ID] = true;
                    key.SetState(KeyPlus.StateInfo.ALREADY_USED);
                    KeyInventory.transform.GetChild(i).gameObject.SetActive(false);
                }

        }
           
            

        if (!IDList.ContainsValue(false)) isLocked = false;
                
    }


    public void LoadData(GameData data)
    {
       bool isUsed;
        if(IDList != null)
        {
           for(int i = 0; i < IDList.Count; i += 1)
            {
                string id = IDList.GetKey(i);
                data.isDoorPlusGoalCompleted.TryGetValue(id, out isUsed );
                if (isUsed) IDList[id] = true;
            }

        }
    }

    public void SaveData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in IDList)
        {
            if (data.isDoorPlusGoalCompleted.ContainsKey(pair.Key))
                data.isDoorPlusGoalCompleted[pair.Key] = pair.Value;
            else if (pair.Key != null && pair.Key != "")data.isDoorPlusGoalCompleted.Add(pair.Key, pair.Value);
            
        }
    }


    public void Interaction()
    {
        if ( isOpeningAllowed && _isOpened)
            OpeningDoorAnimation(rotateDuration);

        else if(isClosingAllowed && !_isOpened)
            ClosingDoorAnimation(rotateDuration);
    }


    // Update is called once per frame
    void Update()
    {
        if (isInteractionAvalible && Input.GetKeyDown(KeyCode.E) && !isSwitchOnCooldown)
        {
            if (isLocked) Try2UnlockDoor();
            else Interaction();

            StartCoroutine(DoorInteractionCooldown());
        }
        
    }

   

    private void OnDrawGizmos()
    {
        if (_colon != null && _doorBody != null)
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
