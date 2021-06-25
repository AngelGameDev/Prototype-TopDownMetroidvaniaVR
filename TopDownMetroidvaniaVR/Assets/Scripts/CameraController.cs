using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 Offset;

    private void Start()
    {
        if (target == null)
        {
            Destroy(this);
            return;
        }

        Offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position = target.position + Offset;
    }
}
