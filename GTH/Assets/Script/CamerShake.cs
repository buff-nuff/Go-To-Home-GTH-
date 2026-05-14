using UnityEngine;
using System.Collections;
public class CamerShake : MonoBehaviour
{
    public static CamerShake Instance;

    Coroutine shakeCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    //shakeCoroutine 는 함수다.
    public void StartShake(float duration2, float magnitude2)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake(duration2, magnitude2));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeCoroutine = null;
    }

}
