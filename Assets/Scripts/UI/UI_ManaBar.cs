using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UI_ManaBar : UI_BarBase
{
    private void OnEnable()
    {
        GameEvents.OnPlayerManaChanged += UpdateBarValue;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerManaChanged -= UpdateBarValue;
    }
}
