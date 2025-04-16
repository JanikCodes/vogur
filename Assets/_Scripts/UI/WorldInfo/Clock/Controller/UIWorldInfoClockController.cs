using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWorldInfoClockController : MonoBehaviour
{
    private TextMeshProUGUI label;

    private void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CalenderService.OnTimeChanged += HandleTimeChange;
    }

    private void OnDisable()
    {
        CalenderService.OnTimeChanged -= HandleTimeChange;
    }

    private void HandleTimeChange(DateTime datetime)
    {
        label.text = datetime.ToShortTimeString();
    }
}
