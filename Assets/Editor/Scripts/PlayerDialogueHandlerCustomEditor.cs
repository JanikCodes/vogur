using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDialogueHandler))]
public class PlayerDialogueHandlerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerDialogueHandler component = (PlayerDialogueHandler)target;

        if (GUILayout.Button("Leave Dialogue"))
        {
            component.ExitDialogue();
        }
    }
}