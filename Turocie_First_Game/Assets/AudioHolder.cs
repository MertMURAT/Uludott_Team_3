using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioHolder" , menuName = "AudioClip List")]
public class AudioHolder : ScriptableObject
{
    [SerializeField] public List<AudioClip> audioList = new List<AudioClip>();
}


