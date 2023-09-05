using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        if (obj.GetComponent<T>() == null) return obj.AddComponent<T>();
        return obj.GetComponent<T>();
    }


}
