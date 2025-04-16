using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task
{
    public TaskType TaskType;
    public Vector3 Position;
    public Transform TargetTransform;
    public float Duration;
    public float PatrolRadius;
}