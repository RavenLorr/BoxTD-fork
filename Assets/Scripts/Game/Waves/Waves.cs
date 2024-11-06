using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Waves", menuName = "ScriptableObject/Waves")]
public class Waves : ScriptableObject
{
    public List<Wave> waves;
}
