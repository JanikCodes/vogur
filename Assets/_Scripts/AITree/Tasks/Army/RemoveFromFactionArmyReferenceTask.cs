using Pathfinding;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Remove From FactionArmyReference", "Tasks/Base/Army/Remove From FactionArmyReference", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class RemoveFromFactionArmyReferenceTask : TaskNode
{
    public static OnArmyDestroyedDelegate OnArmyDestroyed;
    public delegate void OnArmyDestroyedDelegate(Transform armyTransform, string factionId);

    // Stored required components.
    private FactionAssociation factionAssociation;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        factionAssociation = GetOwner().GetComponent<FactionAssociation>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        string factionId = factionAssociation.AssociatedFactionTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id;

        OnArmyDestroyed?.Invoke(GetOwner().transform, factionId);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
