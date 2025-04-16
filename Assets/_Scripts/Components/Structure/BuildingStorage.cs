using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStorage : MonoBehaviour
{
    [field: SerializeField] public List<Building> Buildings { get; set; }
}
