using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Main;

    public Camera RefCamera;
    public Animator RefAnimator;
    
    [Space(10)]
    
    public float Speed = 10f;
    public float TurnSpeed = 10f;

    private Rigidbody RefRigidbody;
    private Vector3 ActiveVelocity;
    private Quaternion TargetRot;

    private Door InteractibleDoor;  // Make a queue/list eventually.

    private void Awake()
    {
        RefRigidbody = GetComponent<Rigidbody>();

        Main = this;
    }

    private void Update()
    {
        if (EndTrigger.IsResetting)
        {
            ActiveVelocity = Vector3.zero;
            RefAnimator.SetFloat("Blend", 0f);
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (InteractibleDoor != null)
            {
                InteractibleDoor.Interact();
            }
        }

        // Get camera y-axis quaternion.
        var cameraForward = Quaternion.AngleAxis(RefCamera.transform.eulerAngles.y, Vector3.up);

        ActiveVelocity = 
            cameraForward * 
            new Vector3
            (
                Input.GetAxis("Horizontal"), 
                0f, 
                Input.GetAxis("Vertical")
            );

        var ActiveSpeed = ActiveVelocity.magnitude;

        ActiveVelocity.Normalize();
        
        // Set target to rotate to face forward, in regards to velocity.
        TargetRot = Quaternion.AngleAxis
        (
            Mathf.Atan2(ActiveVelocity.x, ActiveVelocity.z) * 180f / Mathf.PI,
            Vector3.up
        );
        
        // Ease into target rotation, based on speed.
        transform.localRotation = Quaternion.Lerp
        (
            transform.localRotation,
            TargetRot,
            TurnSpeed * ActiveSpeed * Time.deltaTime
        );

        ActiveVelocity *= ActiveSpeed;
        
        // Animation controls.
        RefAnimator.SetFloat("Blend", ActiveSpeed);
    }

    private void LateUpdate()
    {
        RefRigidbody.velocity = ActiveVelocity * Speed;
    }

    public void SetInteractibleDoor(Door interactibleDoor)
    {
        InteractibleDoor = interactibleDoor;
    }

    public void RemoveInteractibleDoor()
    {
        InteractibleDoor = null;
    }
}
