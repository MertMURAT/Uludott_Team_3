using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Diken : MonoBehaviour
{

    
    Vector2 _detectionSize = new Vector2(10f, 10f);
    float _boxAngle = 0f;
    Vector3 _punchForce = new Vector3(0f, -10f, 0f);
    float _punchEffDuration = 2f;
    int _vibaration = 1;
    float _elasticity = 10f;


    public Vector2 DetectionSize { get { return _detectionSize; } set { _detectionSize = value; } }
    public Vector3 PunchForce { get { return _punchForce; } set { _punchForce = value; } }
    public float PunchEffDuration { get { return _punchEffDuration; } set { _punchEffDuration = value; } }
    public int Vibaration { get { return _vibaration; } set { _vibaration = value; } }
    public float Elasticity { get { return _elasticity; } set { _elasticity = value; } }




    GameObject _detectionBox;


    void Start()
    {
        _detectionBox = transform.GetChildByName("DetectionBox");
    }


    void DetectionByLayerName(string layerName , System.Action<Collider2D> OnDetection )
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            _detectionBox.transform.position, 
            _detectionSize, 
            _boxAngle , 
            LayerMask.GetMask(layerName)
        );

        foreach (Collider2D coll in colls)
            if (OnDetection != null && LayerMask.NameToLayer(layerName) == coll.gameObject.layer) 
                OnDetection(coll);
        

    }

    void OnCharacterDetection(Collider2D coll)
    {
        coll.GetComponent<CharacterController>().Kill();
        transform.DOPunchScale(_punchForce, _punchEffDuration ,_vibaration , _elasticity);
        
    }

    private void FixedUpdate()
    {
        DetectionByLayerName("Character", OnCharacterDetection);
    }


    private void OnDrawGizmos()
    {
        if (_detectionBox != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_detectionBox.transform.position, _detectionSize);
        }
    }
}
