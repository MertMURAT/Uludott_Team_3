using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGridController : MonoBehaviour
{

    // Start is called before the first frame update
    private void OnEnable()
    {
        for (int i = 0; i < gameObject.transform.childCount; i += 1)
            gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = ("Level " + (i+1));
        
    }
}
