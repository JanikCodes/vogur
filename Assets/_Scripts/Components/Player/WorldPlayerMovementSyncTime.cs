using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class WorldPlayerMovementSyncTime : MonoBehaviour
{
    private IAstarAI ai;
    private TimeServiceReference timeServiceReference;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();
        timeServiceReference = GetComponent<TimeServiceReference>();
    }

    private void Update()
    {
        bool isMovementStopped = ai.reachedEndOfPath;

        if (isMovementStopped)
        {
            timeServiceReference.Service.SetTime(TimeState.Paused);
        }
        else
        {
            timeServiceReference.Service.SetTime(TimeState.Playing);
        }
    }
}
