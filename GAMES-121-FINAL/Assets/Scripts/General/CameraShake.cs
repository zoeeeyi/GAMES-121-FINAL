using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float m_shakeDuration;
    [SerializeField] AnimationCurve m_strengthCurve;

    IEnumerator Shake()
    {
        Vector2 _startPos = transform.position;
        float _timer = 0;

        while (_timer < m_shakeDuration)
        {
            _timer += Time.deltaTime;
            float _strength = m_strengthCurve.Evaluate(_timer / m_shakeDuration);
            Debug.Log(_strength);
            transform.position = (Vector2) transform.position + Random.insideUnitCircle * _strength;
            yield return null;
        }

        transform.position = _startPos;
    }
}
