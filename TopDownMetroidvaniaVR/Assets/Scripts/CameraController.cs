using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum EViewMode
    {
        Isometric = 0,
        Perspective = 1,
        TopDown = 2
    }

    public static CameraController Main = null;

    public Transform Target;

    [Space(10)]

    public EViewMode ViewMode;

    [Space(10)]

    public float TurnSpeed;
    public float FollowSpeed;

    [Space(5)]

    [Header("Isometric")]

    public float CameraArmLengthIso;
    public float OrthoMin;
    public float OrthoMax;
    public float ZoomSpeedScrollIso;
    public float ZoomSpeedButtonIso;
    public float MinHeightIso;
    public float MaxHeightIso;
    public float HeightAdjustSpeedIso;

    [Space(5)]

    [Header("Perspective")]

    public float CameraArmLengthPerspective;
    public float ZoomSpeedScrollPerspective;
    public float ZoomSpeedButtonPerspective;
    public float ZoomArmlengthMin;
    public float ZoomArmLengthMax;
    public float MinHeightPerspective;
    public float MaxHeightPerspective;
    public float HeightAdjustSpeedPerspective;

    [Space(10)]

    [Header("TopDown")]

    public float ZoomHeightTopDown;
    public float ZoomSpeedTopDown;
    public float ZoomTopDownMin;
    public float ZoomTopDownMax;

    private Camera RefCamera;

    private Vector3 ActiveOffset;
    private float ActiveAngle;
    private float FixedTargetHeight;
    private float HeightOffsetIso;
    private float HeightOffsetPerspective;
    private Quaternion TargetRot;

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

        HeightOffsetIso = Mathf.Sqrt(transform.position.y);
        HeightOffsetPerspective = 10;

        ActiveAngle = transform.eulerAngles.y + 180f;

        FixedTargetHeight = Target.position.y; // Saves this to use forever.
    }

    private void Update()
    {
        // Switch view mode
        if (Input.GetButtonDown("ToggleView"))
        {
            if (ViewMode == EViewMode.Isometric)
            {
                ViewMode = EViewMode.Perspective;
            }
            else if (ViewMode == EViewMode.Perspective)
            {
                ViewMode = EViewMode.TopDown;
            }
            else if (ViewMode == EViewMode.TopDown)
            {
                ViewMode = EViewMode.Isometric;
            }
        }

        if (ViewMode == EViewMode.Isometric)
        {
            UpdateIsometric();
        }
        else if (ViewMode == EViewMode.Perspective)
        {
            UpdatePerspective();
        }
        else if (ViewMode == EViewMode.TopDown)
        {
            UpdateTopDown();
        }
    }

    private void UpdateIsometric()
    {
        if (!RefCamera.orthographic)
        {
            RefCamera.orthographic = true;
        }

        // Horizontal rotation.
        ActiveAngle += Input.GetAxis("LookHorizontal") * TurnSpeed * Time.deltaTime;

        ActiveOffset = Quaternion.AngleAxis(ActiveAngle, Vector3.up) * (Vector3.forward * CameraArmLengthIso);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);

        // Vertical tilt.
        HeightOffsetIso += Input.GetAxis("LookTilt") * HeightAdjustSpeedIso * Time.deltaTime;
        HeightOffsetIso = Mathf.Clamp(HeightOffsetIso, MinHeightIso, MaxHeightIso);

        RefCamera.orthographicSize += Input.GetAxis("Scrollwheel") * ZoomSpeedScrollIso * Time.deltaTime;
        RefCamera.orthographicSize += Input.GetAxis("Zoom") * ZoomSpeedButtonIso * Time.deltaTime;
        RefCamera.orthographicSize = Mathf.Clamp(RefCamera.orthographicSize, OrthoMin, OrthoMax);
    }

    private void UpdatePerspective()
    {
        if (RefCamera.orthographic)
        {
            RefCamera.orthographic = false;
        }

        // Horizontal rotation.
        ActiveAngle += Input.GetAxis("LookHorizontal") * TurnSpeed * Time.deltaTime;

        ActiveOffset = Quaternion.AngleAxis(ActiveAngle, Vector3.up) * (Vector3.forward * CameraArmLengthPerspective);

        // Vertical tilt.
        HeightOffsetPerspective += Input.GetAxis("LookTilt") * HeightAdjustSpeedPerspective * Time.deltaTime;
        HeightOffsetPerspective = Mathf.Clamp(HeightOffsetPerspective, MinHeightPerspective, MaxHeightPerspective);

        // Zoom with CameraArmLengthPerspective
        CameraArmLengthPerspective += Input.GetAxis("Scrollwheel") * ZoomSpeedScrollPerspective * Time.deltaTime;
        CameraArmLengthPerspective += Input.GetAxis("Zoom") * ZoomSpeedScrollPerspective * Time.deltaTime;
        CameraArmLengthPerspective = Mathf.Clamp(CameraArmLengthPerspective, ZoomArmlengthMin, ZoomArmLengthMax);
    }

    private void UpdateTopDown()
    {
        if (RefCamera.orthographic)
        {
            RefCamera.orthographic = false;
        }

        // Horizontal rotation.
        ActiveAngle += (-1f * Input.GetAxis("LookHorizontal")) * TurnSpeed * Time.deltaTime;

        // Zoom with CameraArmLengthPerspective
        ZoomHeightTopDown += Input.GetAxis("LookTilt") * ZoomSpeedTopDown * Time.deltaTime;
        ZoomHeightTopDown = Mathf.Clamp(ZoomHeightTopDown, ZoomTopDownMin, ZoomTopDownMax);
    }

    private void LateUpdate()
    {
        if (ViewMode == EViewMode.Isometric)
        {
            LateUpdateIsometric();
        }
        else if (ViewMode == EViewMode.Perspective)
        {
            LateUpdatePerspective();
        }
        else if (ViewMode == EViewMode.TopDown)
        {
            LateUpdateTopDown();
        }
    }

    private void LateUpdateIsometric()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight + (HeightOffsetIso * HeightOffsetIso);

        adjustedTargetPos += ActiveOffset;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        transform.LookAt(Target);
    }

    private void LateUpdatePerspective()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight + HeightOffsetPerspective;

        adjustedTargetPos += ActiveOffset;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        // Look at the target (player).
        transform.LookAt(Target.position);
    }

    private void LateUpdateTopDown()
    {
        Vector3 adjustedTargetPos = Target.position;
        adjustedTargetPos.y = FixedTargetHeight + ZoomHeightTopDown;

        // Smoothly move to active target pos.
        transform.position = Vector3.Lerp(transform.position, adjustedTargetPos, FollowSpeed * Time.deltaTime);

        //transform.LookAt(Target);
        transform.localEulerAngles = new Vector3(90f, 0f, ActiveAngle);
    }
}
