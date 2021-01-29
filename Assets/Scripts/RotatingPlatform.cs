using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] RotatingPlatformSwitch rotatingPlatformSwitch;
    [SerializeField] float rotationSpeed;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Pushable"))
            collision.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
            collision.transform.SetParent(null);
    }

    private void Update()
    {
        if(rotatingPlatformSwitch.Trigger)
        {
            transform.eulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
        }
    }
}
