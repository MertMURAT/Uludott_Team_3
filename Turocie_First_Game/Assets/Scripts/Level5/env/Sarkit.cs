using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sarkit : MonoBehaviour
{

    [Header("Settings")]
    private float _distanceY = 13.7f;
    private float _distanceX = 0f;
    private float _roadDuration = 4f;
    private float _vibarationDuration = 4f;

    public float DistanceX { get { return _distanceX; } set { _distanceX = value; } }
    public float DistanceY { get { return _distanceY; } set { _distanceY = value; } }
    public float RoadDuration { get { return _roadDuration; } set { _roadDuration = value; } }
    public float VibarationDuration { get { return _vibarationDuration; } set { _vibarationDuration = value; } }


    LayerMask CharacterLayer;
    float y0;
    bool isFallingDown = false;


    // Start is called before the first frame update
    void Start()
    {
        CharacterLayer = LayerMask.GetMask("Character");
        y0 = transform.position.y;
    }

    void FallDown()
    {
        RaycastHit2D group = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + _distanceX, transform.position.y - _distanceY) ,CharacterLayer);
        if (group.collider != null && !isFallingDown)
        {
            //Debug.Log(group.collider.gameObject.name);
            transform.DOShakePosition(_vibarationDuration ,0.5f).OnComplete( () => {
                    transform.DOMoveY( y0 -_distanceY, _roadDuration)
                    .SetEase(Ease.InExpo)
                    .OnComplete(()=> { Kill(); });
            });


            isFallingDown = true;
        }
        
    }
    public void Kill() { if (gameObject != null) Destroy(gameObject); }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(" " + collision.gameObject.layer + "   //   " + CharacterLayer.value);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character")) {
            //Debug.Log("Hi");
            collision.gameObject.GetComponent<CharacterController>().Kill();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FallDown();
    }



    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + _distanceX, transform.position.y - _distanceY));
    }
}
