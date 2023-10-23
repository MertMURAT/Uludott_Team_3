using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePlus : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Settings")]
    public bool IsOn = false;
    public int CountCollided = 0;
    [SerializeField] List<GameObject> disappearingPlatforms;
    [SerializeField] SpriteRenderer rndr;
    [SerializeField] Sprite isOnSprite;
    [SerializeField] Sprite isOffSprite;

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
        if(CountCollided > 0) IsOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CountCollided -= 1;
        if(CountCollided < 1) IsOn = false;
    }

    private void FixedUpdate()
    {
        if (IsOn)
        {
            foreach (GameObject platform in disappearingPlatforms) platform.SetActive(false);
            rndr.sprite = isOnSprite;
        }
            
            
        else
        {
            foreach (GameObject platform in disappearingPlatforms) platform.SetActive(true);
            rndr.sprite = isOffSprite;
        }
            
            
    }


}
