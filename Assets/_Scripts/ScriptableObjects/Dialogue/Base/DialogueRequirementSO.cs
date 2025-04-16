using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueRequirementSO : DataSO
{
    /// <summary>
    /// Determines if the requirement check should be focused on the player(self) or 'other'
    /// </summary>
    public bool Self;

    public abstract bool CheckRequirements(Transform self, Transform other);
}
