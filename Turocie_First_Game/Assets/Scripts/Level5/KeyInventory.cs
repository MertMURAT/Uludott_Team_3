using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyInventory
{

    GameObject _keyInventory;
    public KeyInventory(GameObject Character)
    {
        this._keyInventory = Character.GetComponent<Transform>().GetChildByName("KeyInventory");
    }

    public bool FindKey(GameObject key)
    {
        return this._keyInventory.IsChildIn(key);
    }

    public bool FindAndUseKey(GameObject goalkey , System.Action<GameObject> Use)
    {
        GameObject foundKey = this._keyInventory.GetIdenticalChild(goalkey);
        if(foundKey != null)
        {
            Use(goalkey);
            return true;
        }
        return false;
    }

}
