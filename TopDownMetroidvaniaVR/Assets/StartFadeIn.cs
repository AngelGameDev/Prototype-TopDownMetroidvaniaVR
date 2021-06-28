using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeIn : MonoBehaviour
{
    public float FadeInDelayInitial = 0.5f;
    public float FadeInTime = 1.5f;
    public float FadeInHeight = 5.2f;
    public AnimationCurve FadeInCurve;

    private Vector3 endPos;

    private void Awake()
    {
        endPos = transform.position;

        transform.position = new Vector3
        (
            transform.position.x,
            transform.position.y + FadeInHeight,
            transform.position.z
        );

        StartCoroutine(RoutineStartFadeIn());
    }

    private IEnumerator RoutineStartFadeIn()
    {
        yield return new WaitForSeconds(FadeInDelayInitial);

        float startTime = Time.time;

        Vector3 startPos = transform.position;

        while (Time.time - startTime < FadeInTime)
        {
            float t = (Time.time - startTime) / FadeInTime;
            t = FadeInCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;

        }

        transform.position = endPos;
    }
}
