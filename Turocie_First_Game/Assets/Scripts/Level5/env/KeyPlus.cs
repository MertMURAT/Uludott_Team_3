using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class KeyPlus : MonoBehaviour , IDataPersistence
{
    public int state = 0;
    
    public string ID;
    [Header("Inventory Object")]
    public GameObject keyInv;


    [Header("Animation Settings")]
    public bool IsCollectingAnimationEnabled = true;
    public float MoveDuration = 2f;
    public float FadeAwayDuration = 1f;
    [Range(-180f, 180f)] public float RotationAmount = 90f;
    public float RotateDuration = 1f;
    public Vector3 KeyOffset = new Vector3(1f, 0f, 0f);
    public Ease EaseType;

    public enum StateInfo
    {
        ON_GROUND = 0,
        IN_INVENTORY = 1 ,
        ALREADY_USED = 2
    };

    public void SetState(StateInfo newState)
    {
        this.state = ((int)newState);
    }

    public void LoadData(GameData data)
    {
        data.keyPlusStates.TryGetValue(ID, out state);
        switch (state)
        {
            case 0: // key is on the ground
                break;

            case 1: // key has been taken
                transform.parent = keyInv.transform;
                CollectingKeyAnimation();
                break;

            case 2: // key is used to open a door
                Kill();
                break;

            default: // ERROR
                break;

        }
    }

    public void SaveData(GameData data)
    {
        
        
        if (data.keyPlusStates.ContainsKey(ID))
        {
            data.keyPlusStates.Remove(ID);
        }
        data.keyPlusStates.Add(ID, state);
        
    }

    public void CollectingKeyAnimation()
    {
            Sequence keyAnim = DOTween.Sequence();
            keyAnim.Append(
                transform.DOLocalMove(
                    Vector3.zero + (keyInv.transform.childCount - 1) * KeyOffset * (Mathf.Pow(-1f , (keyInv.transform.childCount % 2))) , 
                    MoveDuration
                ).SetEase(EaseType));
            keyAnim.Join(transform.DORotate(new Vector3(0f, 0f, RotationAmount), RotateDuration));

    }


    public void Kill()
    {
        SetState(StateInfo.ALREADY_USED);
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision && collision.gameObject && collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            
            transform.parent = keyInv.transform;
            SetState(StateInfo.IN_INVENTORY);
            if(IsCollectingAnimationEnabled) CollectingKeyAnimation();
            Destroy(this.GetComponent<Collider2D>());


        }
    }





}
