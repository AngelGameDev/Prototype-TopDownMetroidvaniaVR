using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomTriggerable : MonoBehaviour
{
    public float initalDrop = 5f;
    
    public float MinRiseSeconds = 0.5f;
    public float MaxRiseSeconds = 1f;

    private Vector3 risenPos;
    private bool HasTriggered = false;

    private void Awake()
    {
        risenPos = transform.position;
        transform.Translate(Vector3.down * initalDrop);
    }

    public void Trigger()
    {
        if (HasTriggered) return;

        StartCoroutine(RoutineRise(Random.Range(MinRiseSeconds, MaxRiseSeconds)));
        
        HasTriggered = true;
    }

    private IEnumerator RoutineRise(float riseTimeSeconds)
    {
        float startTime = Time.time;

        var startPos = transform.position;

        while (Time.time - startTime < riseTimeSeconds)
        {
            float t = (Time.time - startTime) / riseTimeSeconds;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(startPos, risenPos, t);

            yield return t;
        }

        transform.position = risenPos;
    }
}
