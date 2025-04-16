using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeService : MonoBehaviour
{
    [field: SerializeField, ReadOnlyField] public TimeState TimeState { get; private set; }

    private bool isForcedPaused;

    private const float PAUSE_TIME_SCALE = 0f;
    private const float PLAYING_TIME_SCALE = 1f;

    /// <summary>
    /// Is used to modify the time on the world.
    /// </summary>
    /// <param name="force">Determines if <see cref="TimeService.isForcedPaused"/> can be written to.</param>
    public void SetTime(TimeState state, bool force = false)
    {
        TimeState = state;

        switch (state)
        {
            case TimeState.Playing:
                Resume(force);
                break;
            case TimeState.Paused:
                Pause(force);
                break;
        }
    }

    private void Pause(bool force)
    {
        if (force) { isForcedPaused = true; }

        Time.timeScale = PAUSE_TIME_SCALE;
    }

    private void Resume(bool force)
    {
        if (force)
        {
            isForcedPaused = false;
            Time.timeScale = PLAYING_TIME_SCALE;
        }
        else
        {
            if (!isForcedPaused)
            {
                Time.timeScale = PLAYING_TIME_SCALE;
            }
        }
    }
}