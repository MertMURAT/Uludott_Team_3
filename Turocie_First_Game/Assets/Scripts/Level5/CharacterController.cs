using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour , IDataPersistence
{

    Rigidbody2D rbody;
    Transform trf;
    [SerializeField] float _speed = 200f;
    [SerializeField] float JumpModifier = 25f;
    public KeyInventory KeyInv;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }


    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        //data.isLevelCompleted = true;
        data.playerPosition = this.transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
        trf = transform;
        rbody = GetComponent<Rigidbody2D>();
        KeyInv = new KeyInventory(this.gameObject);

    }

    void CharacterMovement() {


        if (Input.GetButtonDown("Jump"))
        {
            rbody.AddForce(new Vector2(0f, JumpModifier), ForceMode2D.Impulse);
            
        }
        float horizontalC = Input.GetAxisRaw("Horizontal");
        rbody.AddForce(new Vector2(_speed * horizontalC, 0f));
        
        

    }

    


    public void Kill()
    {
        if (gameObject != null) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        CharacterMovement();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
