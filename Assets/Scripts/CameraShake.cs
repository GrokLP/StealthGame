using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
    float shakeTimeRemaining;
    float shakePower;
    float shakeFadeTime;
    float shakeRotation;

    public float rotationMultiplier = 7.5f;

    Vector3 startingPosition;
    Quaternion startingRotation;

    private void Start()
    {
        Instance = this;
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartShake(0.1f, 0.1f);
        }

        transform.position = startingPosition;
        transform.rotation = startingRotation;
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

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }

        //transform.rotation = Quaternion.Euler(0, 0, shakeRotation * Random.Range(-1, 1));
    }


    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }

}
