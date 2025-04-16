using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUID : MonoBehaviour
{
    [field: SerializeField, ReadOnlyField] public string Id { get; private set; }

    private bool overwritten = false;

    public void OverwriteId(string newId)
    {
        Id = newId;
        overwritten = true;
    }

    // public void ForceGenerateIdWithOverwrite()
    // {
    //     GenerateId();
    //     overwritten = true;
    // }

    private void Start()
    {
        if (!overwritten)
        {
            GenerateId();
        }
    }

    private void GenerateId()
    {
        Id = Guid.NewGuid().ToString();
    }
}
