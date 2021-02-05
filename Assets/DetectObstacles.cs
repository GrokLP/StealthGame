using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacles : MonoBehaviour
{
    public float targetHeight;
    float timer;
    float distanceToEdge;
    SphereCollider sphereCollider;
    float edgeBuffer = 0.1f;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        distanceToEdge = sphereCollider.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.distance > distanceToEdge + edgeBuffer)
            {
                timer += Time.deltaTime;
                if (timer >= 0.1f)
                {
                    targetHeight = hit.point.y;
                    transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
                    timer = 0;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Pushable"))
        {
            Vector3 offset = collision.transform.up * (collision.transform.localScale.y / 2f) * 1f;
            Vector3 pos = collision.transform.position + offset;

            targetHeight = pos.y;

            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        }
    }

    /*private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            if(timer >= 0.025f)
            {
                targetHeight = 2;
                transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
                timer = 0;
            }
            

            
        }
    }*/
}
