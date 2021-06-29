using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    [Space(10)]

    public float TurnSpeed;
    public float FollowSpeed;

    private Vector3 StartOffset;
    private Vector3 ActiveOffset;
    private float ActiveAngle;
    private float FixedTargetHeight;

    private void Start()
    {
        StartOffset = transform.position - Target.position;
        FixedTargetHeight = Target.position.y; // Saves this to use forever.
    }

    private void Update()
    {
        ActiveAngle += Input.GetAxis("LookHorizontal") * TurnSpeed * Time.deltaTime;

        ActiveOffset = Quaternion.AngleAxis(ActiveAngle, Vector3.up) * StartOffset;
    }

    private void LateUpdate()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight;

        adjustedTargetPos = adjustedTargetPos + ActiveOffset;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        transform.LookAt(Target);
    }
}
