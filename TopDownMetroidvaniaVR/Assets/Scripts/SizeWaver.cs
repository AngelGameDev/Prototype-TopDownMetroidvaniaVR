using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeWaver : MonoBehaviour
{
    public float amplitude;
    public float frequency;

    private Vector3 startSize;

    private void Awake()
    {
        startSize = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = transform.localScale + transform.localScale * (Mathf.Sin(frequency * Time.time) * amplitude);
    }
}
