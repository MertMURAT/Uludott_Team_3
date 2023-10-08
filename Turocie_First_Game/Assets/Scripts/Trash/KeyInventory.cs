using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyInventory : MonoBehaviour
{
    [SerializeField] GameObject _girl;
    [SerializeField] GameObject _doctor;
    [SerializeField] bool _isInvOnDoctor = true;

    BoyMovementController _girlScript;
    PlayerMovementController _doctorScript;

    [Header("Animation Settings")]
    [SerializeField] bool _isAnimationEnabled;
    [SerializeField] float _floatingDuration;
    [SerializeField] Ease _easeType;

    [Header("Local Offset")]
    [SerializeField] Vector3 localOffset;

   

    private void Awake()
    {
        if (
            _girl == null ||
            _doctor == null ||
            !_girl.TryGetComponent<BoyMovementController>(out _girlScript) ||
            !_doctor.TryGetComponent<PlayerMovementController>(out _doctorScript)
          )
            Debug.LogError("KeyInventory Script couldnt initialized properly.");


        localOffset = this.transform.localPosition;
    }



    private void FixedUpdate()
    {
        if ( _girlScript && _girlScript.enabled && _isInvOnDoctor)
        {
            this.transform.parent = _girl.transform;
            if (_isAnimationEnabled)
                this.transform.DOLocalMove(Vector3.zero + localOffset, _floatingDuration).SetEase(_easeType);
            _isInvOnDoctor = false;
        }
        else if ( _doctorScript && _doctorScript.enabled && !_isInvOnDoctor)
        {
            this.transform.parent = _doctor.transform;
            if (_isAnimationEnabled)
                this.transform.DOLocalMove(Vector3.zero + localOffset, _floatingDuration).SetEase(_easeType);
            _isInvOnDoctor = true;
        }
    }


}
