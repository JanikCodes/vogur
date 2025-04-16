using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedPenalty
{
    abstract float GetSpeedPenalty();
    event Action SpeedPenaltyChanged;
}
