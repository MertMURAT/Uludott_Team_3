using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public static class Extensions
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        if (obj.GetComponent<T>() == null) return obj.AddComponent<T>();
        return obj.GetComponent<T>();
    }

    public static GameObject GetChildByName(this Transform tf , string childName)
    {
        for(int i = 0; i < tf.childCount; i += 1)
        {
            if (tf.GetChild(i).name == childName) return tf.GetChild(i).gameObject;
        }
        return null;
    }

    public static GameObject GetIdenticalChild(this GameObject parent , GameObject identicalObj)
    {
        for (int i = 0; i < parent.transform.childCount; i += 1)
            if (parent.transform.GetChild(i).gameObject == identicalObj) 
                return parent.transform.GetChild(i).gameObject;

        return null;
    }

    public static GameObject GetIdenticalChild(this Transform parentTf, GameObject identicalObj)
    {
        for (int i = 0; i < parentTf.childCount; i += 1)
            if (parentTf.GetChild(i).gameObject == identicalObj)
                return parentTf.GetChild(i).gameObject;

        return null;
    }

    public static bool IsChildIn(this GameObject parent , GameObject child)
    {
        for (int i = 0; i < parent.transform.childCount; i += 1)
            if (parent.transform.GetChild(i).gameObject == child) return true;
     
        return false;
    }

    public static bool IsChildIn(this Transform parentTf, GameObject child)
    {
        for (int i = 0; i < parentTf.childCount; i += 1)
            if (parentTf.GetChild(i).gameObject == child) return true;

        return false;
    }

    public static void SetAllChild(this Transform tf , System.Action<GameObject> OnChild)
    {
        for (int i = 0; i < tf.childCount; i += 1)
            OnChild(tf.GetChild(i).gameObject);
        
    }

    public static void ExecuteIfComponentExist<T>(this GameObject gObj , System.Action<GameObject> Execute) where T : Component
    {
        if (gObj && gObj.GetComponent<T>()) Execute(gObj);
    }

    public static void ExecuteIfComponentExist<T>(this GameObject gObj, System.Action<T> Execute) where T : Component
    {
        if(gObj != null)
        {
            T component = gObj.GetComponent<T>();
            if (component) Execute(component);
        }
    }

    public static void ExecuteIfComponentExist<T>(this GameObject gObj, System.Action Execute) where T : Component
    {
        if ( gObj && gObj.GetComponent<T>()) Execute();
    }

    public static void  DetectionBoxByLayer( this GameObject _detectionBox, Vector2 BoxSize ,string layerName, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.transform.position.x, _detectionBox.transform.position.y),
            BoxSize,
            0f,
            LayerMask.GetMask(layerName)
         );
        
        foreach (var coll in colls) OnDetection(coll );

    }

    public static void DetectionBoxByLayer(this GameObject _detectionBox, Vector2 BoxSize, LayerMask layer, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.transform.position.x, _detectionBox.transform.position.y),
            BoxSize,
            0f,
            layer
         ) ;

        foreach (var coll in colls) OnDetection(coll);

    }


    public static void DetectionBox(this GameObject _detectionBox, Vector2 BoxSize, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.transform.position.x, _detectionBox.transform.position.y),
            BoxSize,
            0f
         );
        
        foreach (var coll in colls) OnDetection(coll);

    }

    public static void DetectionBoxByLayer(this Transform _detectionBox, Vector2 BoxSize, string layerName, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.position.x, _detectionBox.position.y),
            BoxSize,
            0f,
            LayerMask.GetMask(layerName)
         );
        
        foreach (var coll in colls) OnDetection(coll);

    }

    public static void DetectionBox(this Transform _detectionBox, Vector2 BoxSize, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.position.x, _detectionBox.position.y),
            BoxSize,
            0f
         );
        
        foreach (var coll in colls) OnDetection(coll);

    }

    //

    public static void DetectionBoxByLayer(this GameObject _detectionBox, Vector2 BoxSize,  string layerName,float angle , System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.transform.position.x, _detectionBox.transform.position.y),
            BoxSize,
            angle,
            LayerMask.GetMask(layerName)
         );

        foreach (var coll in colls) OnDetection(coll);

    }

    public static void DetectionBox(this GameObject _detectionBox, Vector2 BoxSize,float angle, System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.transform.position.x, _detectionBox.transform.position.y),
            BoxSize,
            angle
         );

        foreach (var coll in colls) OnDetection(coll);

    }

    public static void DetectionBoxByLayer(this Transform _detectionBox, Vector2 BoxSize, string layerName,float angle ,System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.position.x, _detectionBox.position.y),
            BoxSize,
            angle,
            LayerMask.GetMask(layerName)
         );

        foreach (var coll in colls) OnDetection(coll);

    }

    public static void DetectionBox(this Transform _detectionBox, Vector2 BoxSize,float angle , System.Action<Collider2D> OnDetection)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(
            new Vector2(_detectionBox.position.x, _detectionBox.position.y),
            BoxSize,
            angle
         );

        foreach (var coll in colls) OnDetection(coll);

    }

    public static string GenerateID(int idLength)
    {
        
        using (var rng = new RNGCryptoServiceProvider())
        {
            var bit_count = idLength * 6;
            var byte_count = ((bit_count + 7) / 8);
            var bytes = new byte[byte_count];
            rng.GetBytes(bytes);
            return System.Convert.ToBase64String(bytes);
        }
    }

}
