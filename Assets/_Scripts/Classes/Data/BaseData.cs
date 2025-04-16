using System;
using UnityEngine;

/// <summary>
/// Class that is used for being inherited by data classes.
/// Provides attributes that every data needs, <see cref="Id"/> and <see cref="Active"/>  
/// </summary>
[Serializable]
public class BaseData
{
    public string Id;
    public bool Active; // Default value for base data is true, this bool is used for modders to deactive base data acting as a soft-delete
}
