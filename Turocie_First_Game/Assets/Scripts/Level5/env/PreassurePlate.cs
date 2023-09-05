using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreassurePlate : MonoBehaviour
{

    [SerializeField] GameObject[] _doorGroup;
    [SerializeField] Vector2 _detectionSize;
    [SerializeField] GameObject _detectionBox;
    [SerializeField] Vector2 GrowingConstant = new Vector2(1.33f , 1.33f);
    [SerializeField] Vector2 ShrinkingConstant = new Vector2(0.25f, 0.25f);
    [SerializeField] float ScalingDuration = 0.6f;


    LayerMask _characterLayer;
    bool _isPressed = false;
    float _scaleX0, _scaleY0;


    void PreassurePlateLogic()
    {
        Collider2D[] coll = Physics2D.OverlapBoxAll(_detectionBox.transform.position , _detectionSize , 0f ,_characterLayer);
        if (coll.Length != 0 && !_isPressed)
        {
            transform.DOScale(new Vector3(_scaleX0 * GrowingConstant.x, _scaleY0 * ShrinkingConstant.y) , ScalingDuration);
            _isPressed = true;
        }
        else if(coll.Length == 0 && _isPressed)
        {
            transform.DOScale(new Vector3(_scaleX0 * ShrinkingConstant.x, _scaleY0 * GrowingConstant.y), ScalingDuration);
            _isPressed = false;
        }
    }

    void Awake()
    {
        _characterLayer = LayerMask.GetMask("Character");
        _scaleX0 = transform.localScale.x;
        _scaleY0 = transform.localScale.y;
        
    }

    private void FixedUpdate()
    {
        PreassurePlateLogic();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_detectionBox.transform.position, _detectionSize);
    }

}
