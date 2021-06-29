using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    [Space(10)]

    public float TurnSpeed;
    public float FollowSpeed;

    [Space(5)]

    public float MinHeight;
    public float MaxHeight;
    public float HeightAdjustSpeed;

    [Space(5)]

    public float OrthoMin;
    public float OrthoMax;
    public float ZoomSpeedScroll;

    private Camera RefCamera;

    private Vector3 StartOffset;
    private Vector3 ActiveOffset;
    private float ActiveAngle;
    private float FixedTargetHeight;
    private float HeightOffset;

    private void Start()
    {
        RefCamera = GetComponent<Camera>();

        StartOffset = transform.position - Target.position;
        FixedTargetHeight = Target.position.y; // Saves this to use forever.
    }

    private void Update()
    {
        // Horizontal rotation.
        ActiveAngle += Input.GetAxis("LookHorizontal") * TurnSpeed * Time.deltaTime;

        ActiveOffset = Quaternion.AngleAxis(ActiveAngle, Vector3.up) * StartOffset;

        // Vertical tilt.
        HeightOffset += Input.GetAxis("LookTilt") * HeightAdjustSpeed * Time.deltaTime;
        HeightOffset = Mathf.Clamp(HeightOffset, MinHeight, MaxHeight);

        RefCamera.orthographicSize += Input.GetAxis("Scrollwheel") * ZoomSpeedScroll * Time.deltaTime;
        RefCamera.orthographicSize = Mathf.Clamp(RefCamera.orthographicSize, OrthoMin, OrthoMax);
    }

    private void LateUpdate()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight + HeightOffset;

        adjustedTargetPos = adjustedTargetPos + ActiveOffset;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        transform.LookAt(Target);
    }
}
