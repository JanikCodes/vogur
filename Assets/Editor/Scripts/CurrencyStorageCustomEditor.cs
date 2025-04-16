using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurrencyStorage))]
public class CurrencyStorageCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CurrencyStorage component = (CurrencyStorage)target;

        if (GUILayout.Button("Add 1000"))
        {
            component.ModifyCurrency(1000);
        }

        if (GUILayout.Button("Remove 1000"))
        {
            component.ModifyCurrency(1000);
        }
    }
}