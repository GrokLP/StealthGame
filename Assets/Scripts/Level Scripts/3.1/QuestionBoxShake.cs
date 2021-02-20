using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxShake : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CameraShake.Instance.StartShake(0.1f, 0.1f);
        AudioManager.Instance.PlaySound("HardCollision");
    }

}
