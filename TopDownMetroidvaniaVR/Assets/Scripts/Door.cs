using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: This should be an "interactible in range" interface, if this weren't a dirty prototype.

public class Door : MonoBehaviour
{
    public Transform Hinge;
    public TriggerRoom TriggeredRoom;

    [Space(10)]

    public float CloseAngle = 0f;
    public float OpenAngle = 170f;

    [Space(5)]

    public float AnimTimeOpen;
    public float AnimTimeClose;
    
    [Space(5)]
    
    public AnimationCurve DoorOpenCurve;
    public AnimationCurve DoorCloseCurve;

    [Space(5)]

    public MeshRenderer SphereMeshToDisable;

    private bool IsOpening;
    private bool IsOpen;
    private bool IsTriggered;

    private void Awake()
    {
        if (SphereMeshToDisable != null)
        {
            SphereMeshToDisable.enabled = false;
        }
    }

    public void Interact()
    {
        if (IsOpening)
        {
            return;
        }

        if (!IsOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        Debug.Log("Opening door.");

        if (!IsTriggered)
        {
            TriggeredRoom.Trigger();
            IsTriggered = true;
        }

        StartCoroutine(OpenRoutine());
    }

    public void Close()
    {
        Debug.Log("Closing door.");

        StartCoroutine(CloseRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        IsOpening = true;

        float startTime = Time.time;

        while ((Time.time - startTime) < AnimTimeOpen)
        {
            float t = DoorOpenCurve.Evaluate((Time.time - startTime) / AnimTimeOpen);
            
            Hinge.localEulerAngles = new Vector3
            (
                Hinge.localEulerAngles.x,
                Mathf.Lerp(CloseAngle, OpenAngle, t),
                Hinge.localEulerAngles.z
            );

            yield return null;
        }

        Hinge.localEulerAngles = new Vector3
        (
            Hinge.localEulerAngles.x,
            OpenAngle,
            Hinge.localEulerAngles.z
        );

        IsOpen = true;
        IsOpening = false;
    }

    private IEnumerator CloseRoutine()
    {
        IsOpening = true;

        float startTime = Time.time;

        while ((Time.time - startTime) < AnimTimeClose)
        {
            float t = DoorCloseCurve.Evaluate((Time.time - startTime) / AnimTimeClose);

            Hinge.localEulerAngles = new Vector3
            (
                Hinge.localEulerAngles.x,
                Mathf.Lerp(OpenAngle, CloseAngle, t),
                Hinge.localEulerAngles.z
            );

            yield return null;
        }

        Hinge.localEulerAngles = new Vector3
        (
            Hinge.localEulerAngles.x,
            CloseAngle,
            Hinge.localEulerAngles.z
        );

        IsOpen = false;
        IsOpening = false;
    }
}
