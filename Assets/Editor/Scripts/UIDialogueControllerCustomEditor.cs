using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIDialogueController))]
public class UIDialogueControllerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIDialogueController component = (UIDialogueController)target;

        if (GUILayout.Button("Call Window"))
        {
            component.CallDialogueWindow();
        }

        if (GUILayout.Button("Dismiss Window"))
        {
            component.DismissDialogueWindow();
        }
    }
}