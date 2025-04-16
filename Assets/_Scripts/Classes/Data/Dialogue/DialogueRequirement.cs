using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="DialogueRequirementSO"/> at runtime from.
/// </summary>
[Serializable]
public class DialogueRequirementData
{
    public string Type;
    public Dictionary<string, object> Properties;
}