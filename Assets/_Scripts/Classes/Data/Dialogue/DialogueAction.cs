using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="DialogueActionSO"/> at runtime from.
/// </summary>
[Serializable]
public class DialogueActionData
{
    public string Type;
    public Dictionary<string, object> Properties;
}