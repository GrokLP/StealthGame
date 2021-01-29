using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    [SerializeField] Transform doorOne;
    [SerializeField] Transform doorTwo;

    [SerializeField] float closedPoint;
    [SerializeField] float openPoint;
    [SerializeField] float moveSpeed;

    bool openDoor; 

    private void Update()
    {
        if (openDoor)
            OpenDoor();
        else if(!openDoor)
            CloseDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") | other.CompareTag("Enemy"))
            openDoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") | other.CompareTag("Enemy"))
            openDoor = false;
    }


    void OpenDoor()
    {
        if (doorOne.transform.position.y >= openPoint && doorTwo.transform.position.y >= openPoint)
        {
            doorOne.transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
            doorTwo.transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    void CloseDoor()
    {
        if (doorOne.transform.position.y <= closedPoint && doorTwo.transform.position.y <= closedPoint)
        {
            doorOne.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            doorTwo.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

}
