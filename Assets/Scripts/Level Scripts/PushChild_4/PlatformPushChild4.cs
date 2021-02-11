using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPushChild4 : MonoBehaviour
{
    [SerializeField] PlatformTriggerPushChild4 platformTrigger;
    [SerializeField] float loweredPoint;
    [SerializeField] float raisedPoint;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        if (platformTrigger.RaisePlatform)
            RaisePlatform();
        else if (!platformTrigger.RaisePlatform)
            LowerPlatform();
    }

    void RaisePlatform()
    {
        if (transform.position.y <= raisedPoint)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    void LowerPlatform()
    {
        if (transform.position.y >= loweredPoint)
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }
    }
}
