using System;
using UnityEngine;

/// <summary>
/// Class that is used for being inherited by data scriptableobjects.
/// Provides attributes that every data needs, <see cref="Id"/>  
/// </summary>
[Serializable]
public class DataSO : ScriptableObject
{
    public string Id;
}