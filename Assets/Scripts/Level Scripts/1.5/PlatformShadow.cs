using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PlatformShadow : MonoBehaviour
{
    [SerializeField] Rectangle fakeShadow;
    [SerializeField] LayerMask mask;

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, mask) && fakeShadow != null)
        {
            fakeShadow.transform.position = hit.point + new Vector3(0, 0.01f, 0);
        }
    }
}
