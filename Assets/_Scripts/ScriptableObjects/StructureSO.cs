using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StructureSO : DataSO
{
    public string Name;
    public string Description;
    public string InitiallyOwnedByFaction;
    public Vector3 Position;
    public Quaternion Rotation;
    public GameObject AssignedPrefab;
    public DialogueSO AssignedDialogue;
    public List<BuildingSO> PossibleBuildings = new();
}
