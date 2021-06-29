using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Main = null;

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
    public float ZoomSpeedButton;

    private Camera RefCamera;

    private float CameraArmLength;
    private Vector3 ActiveOffset;
    private float ActiveAngle;
    private float FixedTargetHeight;
    private float HeightOffset;

    private void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
            return;
        }

        Main = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        RefCamera = GetComponent<Camera>();

        HeightOffset = Mathf.Sqrt(transform.position.y);
        ActiveAngle = transform.eulerAngles.y + 180f;

        CameraArmLength = Vector3.Distance
        (
            new Vector3
            (
                transform.position.x,
                0f,
                transform.position.z
            ),
            new Vector3
            (
                Target.position.x,
                0f,
                Target.position.z
            )
        );

        Debug.Log(CameraArmLength);

        FixedTargetHeight = Target.position.y; // Saves this to use forever.
    }

    private void Update()
    {
        // Horizontal rotation.
        ActiveAngle += Input.GetAxis("LookHorizontal") * TurnSpeed * Time.deltaTime;

        ActiveOffset = Quaternion.AngleAxis(ActiveAngle, Vector3.up) * (Vector3.forward * CameraArmLength);

        // Vertical tilt.
        HeightOffset += Input.GetAxis("LookTilt") * HeightAdjustSpeed * Time.deltaTime;
        HeightOffset = Mathf.Clamp(HeightOffset, MinHeight, MaxHeight);

        RefCamera.orthographicSize += Input.GetAxis("Scrollwheel") * ZoomSpeedScroll * Time.deltaTime;
        RefCamera.orthographicSize += Input.GetAxis("Zoom") * ZoomSpeedButton * Time.deltaTime;
        RefCamera.orthographicSize = Mathf.Clamp(RefCamera.orthographicSize, OrthoMin, OrthoMax);
    }

    private void LateUpdate()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight + (HeightOffset * HeightOffset);

        adjustedTargetPos += ActiveOffset;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        transform.LookAt(Target);
    }
}
