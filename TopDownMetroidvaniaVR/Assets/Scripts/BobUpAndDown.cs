using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobUpAndDown : MonoBehaviour
{
    public float Frequency = 1f;
    public float Amplitude = 1f;

    private float StartHeight;

    private void Start()
    {
        StartHeight = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3
        (
            transform.position.x,
            StartHeight + Mathf.Abs(Mathf.Sin(Frequency * Time.time) * Amplitude),
            transform.position.z
        );
    }
}
