using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObject/Wave")]
public class Wave : ScriptableObject
{
    public List<GameObject> enemies;
    public List<Vector3> waveSection;
}
