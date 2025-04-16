using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [field: SerializeField, ReadOnlyField] public DialogueSO Dialogue { get; set; }
}
