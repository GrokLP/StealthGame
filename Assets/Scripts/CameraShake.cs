using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
    float shakeTimeRemaining;
    float shakePower;
    float shakeFadeTime;

    Vector3 startingPosition;

    private void Start()
    {
        Instance = this;
        startingPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startingPosition;
    }
    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1, 1) * shakePower;
            float yAmount = Random.Range(-1, 1) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0, shakeFadeTime * Time.deltaTime);
        }
    }


    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;
    }

}
