using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Random Structure Position", "Tasks/Base/Faction/Get Random Structure Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetRandomStructurePositionTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private BoolKey factionOwnedStructuresOnly;

    [SerializeField]
    private IntKey radius;

    [SerializeField]
    private IntKey minRadius;

    [SerializeField]
    [NonLocal]
    private Vector3Key structurePosition;

    // Stored required components.
    private ObjectStorage objectStorage;
    private StructureServiceReference structureService;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        objectStorage = GetOwner().GetComponent<ObjectStorage>();
        structureService = GetOwner().GetComponent<StructureServiceReference>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        FactionSO factionData = objectStorage.GetObject<FactionSO>();
        if (!factionData)
        {
            return State.Failure;
        }

        List<Transform> structures;
        if (factionOwnedStructuresOnly)
        {
            structures = structureService.Service.GetFactionOwnedStructures(factionData);
        }
        else
        {
            structures = structureService.Service.GetAllStructureTransforms();
        }

        if (structures.Count == 0)
        {
            return State.Failure;
        }

        // get random available structure
        Transform structureTransform = Util.GetRandomValue<Transform>(structures);
        // get random position around a specific radius from that said structure
        Vector3 calculatedPos = Util.GetRandomPositionInRadius(structureTransform.position, radius.GetValue(), minRadius.GetValue());
        // calculate the correct Y position based on the terrain
        calculatedPos = new Vector3(calculatedPos.x, Terrain.activeTerrain.SampleHeight(calculatedPos), calculatedPos.z);

        structurePosition.SetValue(calculatedPos);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}