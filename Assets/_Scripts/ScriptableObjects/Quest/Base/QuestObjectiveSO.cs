using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class to create any custom Quest Objective like <see cref="QuestVisitStructureTypeSO"/>.
/// </summary>
[Serializable]
public abstract class QuestObjectiveSO : ScriptableObject, IQuestObjective
{
    public string Text;
    public string Type;

    /// <summary>
    /// Can be used to subscribe to certain game events in order to update quest objective progress.
    /// </summary>
    public abstract void Initialize(QuestObjective questObjective);
    /// <summary>
    /// Must contain logic to determine if a quest is completed or not.
    /// </summary>
    public abstract bool IsComplete(QuestObjective questObjective);
    /// <summary>
    /// Contains logic that will be executed every tick if the quest step is active
    /// </summary>
    public abstract void UpdateObjective(QuestObjective questObjective);
}

public interface IQuestObjective
{
    bool IsComplete(QuestObjective questObjective);
    void Initialize(QuestObjective questObjective);
    void UpdateObjective(QuestObjective questObjective);
}