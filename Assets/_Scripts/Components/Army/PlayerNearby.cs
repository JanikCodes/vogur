using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearby : MonoBehaviour
{
    [field: SerializeField] public bool IsPlayerNearby { get; private set; }

    private void Awake()
    {
        // set to false by default
        IsPlayerNearby = false;
    }

    public void SetValue(bool value)
    {
        IsPlayerNearby = value;
    }
}
