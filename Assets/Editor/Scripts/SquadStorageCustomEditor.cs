using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SquadStorage))]
public class SquadStorageCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SquadStorage component = (SquadStorage)target;

        GUILayout.Label("Soldier Count: " + component.GetTotalSoldierCount());

        base.OnInspectorGUI();
    }
}