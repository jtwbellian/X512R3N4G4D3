using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorInfo")]
public class ActorInfo : ScriptableObject
{
    public string myName;
    public AudioClip[] audioLines;
    public Transform target;
}
