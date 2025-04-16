using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(IAstarAI), typeof(AIDestinationSetter))]
public class WorldPlayerMovement : MonoBehaviour
{
    private IAstarAI ai;
    private AIDestinationSetter aIDestinationSetter;
    private WorldPlayerInput input;

    private void Awake()
    {
        input = WorldPlayerInput.Instance;

        ai = GetComponent<IAstarAI>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void OnEnable()
    {
        input.OnSingleClick += SingleMouseClick;
        PlayerDialogueHandler.OnDialogueDismiss += HandleDialogueDismiss;
    }

    private void OnDisable()
    {
        input.OnSingleClick -= SingleMouseClick;
        PlayerDialogueHandler.OnDialogueDismiss -= HandleDialogueDismiss;
    }

    private void SingleMouseClick()
    {
        // TODO: check here if we can even move? Or do that elsewhere? perhaps I shoot an event and try listening for a callback or smth? So any script that does not want me to be able to move can return false or smth?

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(input.GetMousePosition());
        if (Physics.Raycast(ray, out hit, 1000))
        {
            DialogueTrigger dialogueTrigger = hit.transform.GetComponent<DialogueTrigger>();
            if (dialogueTrigger)
            {
                // follow target
                aIDestinationSetter.target = hit.transform;
            }
            else
            {
                // free movement
                SetCustomDestination(hit.point);
            }
        }
    }

    private void HandleDialogueDismiss(Transform self, Transform other)
    {
        // reset target if we leave the dialogue
        SetCustomDestination(transform.position);
    }

    private void SetCustomDestination(Vector3 position)
    {
        ai.destination = position;

        // clear target
        aIDestinationSetter.target = null;
    }
}
