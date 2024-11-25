using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialPosition;

    public IEnumerator Shake(float duration, float magnitude)
    {
        initialPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

       
            transform.position = initialPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = initialPosition;
    }
}
