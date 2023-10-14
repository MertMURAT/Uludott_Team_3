using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedButton : MonoBehaviour
{



    
    [SerializeField] GameObject[] ConnectedObjects; 

    [Header("Settings")]
    public bool isOn = false;
    public bool UseWithNoCooldown = false;

    [Header("Detection Settings")]
    [SerializeField] GameObject _detectionBox;
    public Vector2 BoxSize = Vector2.one;
    public bool IsWithinDetectionRange;
    

    [Header("Cooldown")]
    public bool isOnCooldown = false;
    public float CooldownDuration = 1f;

    

    // Start is called before the first frame update
    void Start()
    {
        _detectionBox = transform.GetChild(0).gameObject;
    }

    public void OnActive()
    {
        for(int i = 0; i < ConnectedObjects.Length; i += 1)
        {
            GameObject cObj = ConnectedObjects[i];
            cObj.ExecuteIfComponentExist<RollingStone>((RollingStone stone) => {stone.isActivated = true;});
        }
    }

    IEnumerator SwitchCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        isOnCooldown = false;

    }

    void SwitchBehaviour()
    {
        isOn = !isOn;
    }

    void SwitchBehaviorWithCooldown()
    {
        if (!isOnCooldown)
        {
            SwitchBehaviour();
            StartCoroutine(SwitchCooldown());
        }
    }
    void DetectionBoxByLayer(string layerName, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll( 
            new Vector2(_detectionBox.transform.position.x , _detectionBox.transform.position.y) , 
            BoxSize, 
            0f,
            LayerMask.GetMask(layerName)
         );
        IsWithinDetectionRange = false;
        foreach (var coll in colls) OnDetection(coll);
        
    }

    void InputManager( KeyCode Key ,System.Action OnInput)
    {
        if (IsWithinDetectionRange && Input.GetKeyDown(Key)) OnInput();   
    }


    private void FixedUpdate()
    {
        DetectionBoxByLayer("Character", (Collider2D coll) =>{

            
            if (coll && coll.gameObject)
                coll.gameObject.ExecuteIfComponentExist<CharacterController>(() => { IsWithinDetectionRange = true; Debug.Log(coll); });
            
       });
    }


    // Update is called once per frame
    void Update()
    {

        
        
        if (UseWithNoCooldown) InputManager(KeyCode.E, SwitchBehaviour);
        else InputManager(KeyCode.E, SwitchBehaviorWithCooldown);

        if (isOn) OnActive();
        
    }

    private void OnDrawGizmos()
    {
        
        if (_detectionBox)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_detectionBox.transform.position, BoxSize);
        }
    }
}
