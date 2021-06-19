using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform Hinge;
    
    [Space(10)]
    
    public float CloseAngle = 0f;
    public float OpenAngle = 170f;
    
    [Space(10)]
    
    public float AnimTimeOpen;
    public float AnimTimeClose;
    
    [Space(10)]
    
    public AnimationCurve DoorOpenCurve;
    public AnimationCurve DoorCloseCurve;

    public void Open()
    {
        Hinge.localEulerAngles = new Vector3
        (
            0f,
            OpenAngle,
            0f
        );
    }

    public void Close()
    {
        Hinge.localEulerAngles = new Vector3
        (
            0f,
            CloseAngle,
            0f
        );
    }

    // private IEnumerator OpenRoutine()
    // {
    //     yield break;
    // }
    //
    // private IEnumerator CloseRoutine()
    // {
    //     yield break;
    // }
}
