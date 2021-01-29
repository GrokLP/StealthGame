using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
    }
}
