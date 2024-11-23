using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialPosition;

    public IEnumerator Shake(float duration, float magnitude)
    {
        // Store the current position of the camera at the start of the shake
        initialPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Offset the camera's initial position
            transform.position = initialPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Restore to the current position (not the original position when Shake started)
        transform.position = initialPosition;
    }
}
