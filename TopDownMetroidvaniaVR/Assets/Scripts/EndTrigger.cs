using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public static bool IsResetting;

    public Transform RefGroundplane;

    public float FadeOutHeight;
    public float FadeOutTime;
    public AnimationCurve FadeOutCurve;

    public float AfterFadeWaitSeconds;

    private void Awake()
    {
        IsResetting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsResetting)
        {
            return;
        }

        IsResetting = true;
        
        StartCoroutine(FadeOutScene());
    }

    private IEnumerator FadeOutScene()
    {
        float startTime = Time.time;

        Vector3 startPos = RefGroundplane.position;
        Vector3 endPos = new Vector3
        (
            RefGroundplane.position.x,
            RefGroundplane.position.y + FadeOutHeight,
            RefGroundplane.position.z
        );

        while (Time.time - startTime < FadeOutTime)
        {
            float t = (Time.time - startTime) / FadeOutTime;
            t = FadeOutCurve.Evaluate(t);

            RefGroundplane.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        RefGroundplane.position = endPos;

        yield return new WaitForSeconds(AfterFadeWaitSeconds);

        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
