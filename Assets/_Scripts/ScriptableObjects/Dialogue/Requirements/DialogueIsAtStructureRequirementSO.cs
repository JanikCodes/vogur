using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIsAtStructureRequirementSO : DialogueRequirementSO
{
    public string StructureId;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        // Transform target = Self ? self : other;
        ObjectStorage objectStorage = other.GetComponent<ObjectStorage>();
        StructureSO structureSO = objectStorage.GetObject<StructureSO>();

        // we are not even at a structure
        if (structureSO == null) { return false; }

        return structureSO.Id.Equals(StructureId);
    }
}
