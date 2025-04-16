using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDialogueHandler))]
public class DialogueImmunity : MonoBehaviour
{
    [field: SerializeField, ReadOnlyField] public bool Immune { get; private set; }

    private Coroutine immunityCoroutine;

    public void SetImmunity(float time)
    {
        if (immunityCoroutine != null)
        {
            StopCoroutine(immunityCoroutine);
        }

        immunityCoroutine = StartCoroutine(MakeImmune(time));
    }

    private IEnumerator MakeImmune(float time)
    {
        Immune = true;

        yield return new WaitForSeconds(time);

        Immune = false;
    }
}
