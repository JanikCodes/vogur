using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Component that can be attached to Gameobjects in order to have a reference to the main faction Transform to access other components
/// </summary>
public class FactionAssociation : MonoBehaviour
{
    [field: SerializeField, ReadOnlyField] public Transform AssociatedFactionTransform { get; set; }
}
