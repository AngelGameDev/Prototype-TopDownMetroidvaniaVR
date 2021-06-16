using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public TriggerRoom TargetRoom;

    private void OnTriggerEnter(Collider other)
    {
        TargetRoom.Trigger();

        // Disable so it doesn't keep triggering.
        GetComponent<BoxCollider>().enabled = false;
    }
}
