using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    private Door ParentDoor;

    private void Awake()
    {
        ParentDoor = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Main.SetInteractibleDoor(ParentDoor);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.Main.RemoveInteractibleDoor();
    }
}
