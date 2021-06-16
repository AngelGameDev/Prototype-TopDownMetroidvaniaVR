using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoom : MonoBehaviour
{
    private bool HasTriggered = false;

    public void Trigger()
    {
        if (HasTriggered) return;

        var roomTriggerables = GetComponentsInChildren<RoomTriggerable>();

        foreach (var triggerable in roomTriggerables)
        {
            triggerable.Trigger();
        }
        
        Debug.Log("Triggered room (" + gameObject.name + ")");

        HasTriggered = true;
    }
}
