using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlateWithImpulse : MonoBehaviour
{
    [Header("Settings")]
    public bool IsOn = false;
    public int CountCollided = 0;
    [SerializeField] List<GameObject> _objects2ApplyForce;
    [SerializeField] List<Vector2> _force2ApplyObjects;
    [SerializeField] SpriteRenderer rndr;
    [SerializeField] Sprite isOnSprite;
    [SerializeField] Sprite isOffSprite;
    [SerializeField] ForceMode2D _fmode;
    public bool isPushOnlyOnce = true;
    public bool isPushed = false;

    private void Awake()
    {
        rndr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CountCollided += 1;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CountCollided > 0) IsOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CountCollided -= 1;
        if (CountCollided < 1) IsOn = false;
    }


    void PushObjects()
    {
            int i = 0;
            while(i < _objects2ApplyForce.Count)
            {
                Rigidbody2D rb = _objects2ApplyForce[i].GetComponent<Rigidbody2D>();
                rb.AddForce(_force2ApplyObjects[i], _fmode);
                i += 1;
            }
            isPushed = true;

    }

    private void FixedUpdate()
    {
        if (IsOn)
        {
            if (isPushOnlyOnce)
            {
                if (!isPushed) PushObjects();
            }
            else PushObjects();
            rndr.sprite = isOnSprite;
        }


        else
        {
            rndr.sprite = isOffSprite;
        }


    }
}
