using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueActionSO : DataSO
{
    /// <summary>
    /// Determines if the action should be effecting the player(self) or 'other'
    /// </summary>
    public bool Self;

    public abstract void ExecuteEffect(Transform self, Transform other);
}
