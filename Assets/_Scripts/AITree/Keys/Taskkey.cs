using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[KeyColor(0.231f, 0.5f, 0.27f)]
public class TaskKey : Key<Task>
{
    public override bool Equals(Task other)
    {
        return GetValue() == other;
    }
}

// Query implemetation.
[Serializable]
public class TaskQuery : KeyQuery<TaskKey>
{
    [SerializeField]
    protected Task comparer;

    protected override bool Result(TaskKey key)
    {
        return key.GetValue() == comparer;
    }
}

// Receiver implemetation.
public class TaskReceiver : KeyReceiver<TaskKey>
{
    [SerializeField]
    private Task value;

    protected override void Apply(TaskKey key)
    {
        key.SetValue(value);
    }
}