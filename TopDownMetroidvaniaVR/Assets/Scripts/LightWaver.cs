using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWaver : MonoBehaviour
{
    public float baseValue;
    public float amplitude;
    public float frequency;

    private Light refLight;

    private void Awake()
    {
        refLight = GetComponent<Light>();
    }

    private void Update()
    {
        refLight.intensity = baseValue + (Mathf.Sin(frequency * Time.time) * amplitude);
    }
}
