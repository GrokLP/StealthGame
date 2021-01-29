using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = mainCam.transform.rotation;

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
