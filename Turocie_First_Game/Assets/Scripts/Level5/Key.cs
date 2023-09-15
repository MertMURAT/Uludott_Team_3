using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour , IDataPersistence
{
    Collider2D _coll;

    [Header("Configurations for saving system")]
    public bool isCollected = false;
    public bool isInInventory = false;
    [SerializeField] string _id;
    [SerializeField] GameObject Character;

    [Header("Animation Settings")]
    public float MoveDuration = 2f;
    public float FadeAwayDuration = 1f;
    [Range(-180f , 180f)] public float RotationAmount = 90f;
    public float RotateDuration = 1f;
    public Vector3 KeyOffset = new Vector3(3f, 0f, 0f);


    int _idLength = 38;
    [ContextMenu("Generate ID")]
    void GenerateID()
    {
        _id = Extensions.GenerateID(_idLength);
    }

    public void LoadData(GameData data)
    {
        data.isKeyCollected.TryGetValue(_id, out isCollected);
        data.isKeyInKeyInventory.TryGetValue(_id, out isInInventory);
        if (isInInventory)
        {
            GameObject keyInv = Character.transform.GetChildByName("KeyInventory");
            transform.parent = keyInv.transform;

            Sequence keyAnim = DOTween.Sequence();
            keyAnim.Append(transform.DOLocalMove(Vector3.zero + (keyInv.transform.childCount - 1) * KeyOffset, MoveDuration).SetEase(Ease.InOutExpo));
            keyAnim.Join(transform.DORotate(new Vector3(0f, 0f, RotationAmount), RotateDuration));

        }
        else if (isCollected)
            Destroy(this.gameObject);
        
    }

    public void SaveData(GameData data)
    {
        if (data.isKeyCollected.ContainsKey(_id)) data.isKeyCollected.Remove(_id);
        if (data.isKeyInKeyInventory.ContainsKey(_id)) data.isKeyInKeyInventory.Remove(_id);

        data.isKeyCollected.Add(_id, isCollected);
        data.isKeyInKeyInventory.Add(_id, isInInventory);

    }

    private void Awake()
    {
        _coll = gameObject.GetOrAddComponent<CircleCollider2D>();
        _coll.isTrigger = true;
    }

    public void FadeAway()
    {
        transform.GetComponent<SpriteRenderer>().DOFade(0f, FadeAwayDuration).OnComplete(()=> { if (gameObject != null) Destroy(gameObject); });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            isCollected = true;
            isInInventory = true;
            GameObject keyInv = collision.transform.GetChildByName("KeyInventory");
            transform.parent = keyInv.transform;

            Sequence keyAnim = DOTween.Sequence();
            keyAnim.Append(transform.DOLocalMove(Vector3.zero + (keyInv.transform.childCount-1) * KeyOffset, MoveDuration).SetEase(Ease.InOutExpo));
            keyAnim.Join(transform.DORotate(new Vector3(0f, 0f, RotationAmount), RotateDuration));

            Destroy(_coll);
        }
    }
}
