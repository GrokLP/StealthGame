using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraPosSouth;
    [SerializeField] Vector3 cameraPosEast;
    [SerializeField] Vector3 cameraPosNorth;
    [SerializeField] Vector3 cameraPosWest;

    public int camRotation = 45;

    private void Update()
    {
        if(Input.GetButtonDown("RotateRight"))
        {
            if (camRotation > 45)
                camRotation -= 90;
            else
                camRotation = 315;

            RotateCamera(camRotation);
            MoveCamera(camRotation);
        }

        if(Input.GetButtonDown("RotateLeft"))
        {
            if (camRotation < 315)
                camRotation += 90;
            else
                camRotation = 45;

            RotateCamera(camRotation);
            MoveCamera(camRotation);
        }

        
    }

    void RotateCamera(int camRotation)
    {
        mainCamera.transform.DORotate(new Vector3(30, camRotation, 0), 0.5f, RotateMode.Fast);
    }

    void MoveCamera(int camRotation)
    {
        switch(camRotation)
        {
            case (45):
                mainCamera.transform.DOMove(cameraPosSouth, 0.5f);
                break;
            case (135):
                mainCamera.transform.DOMove(cameraPosWest, 0.5f);
                break;
            case (225):
                mainCamera.transform.DOMove(cameraPosNorth, 0.5f);
                break;
            case (315):
                mainCamera.transform.DOMove(cameraPosEast, 0.5f);
                break;
        }
    }
}
