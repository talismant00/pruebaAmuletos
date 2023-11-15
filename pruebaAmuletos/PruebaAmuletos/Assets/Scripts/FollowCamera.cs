using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Aquí debes arrastrar el transform del personaje en el Inspector.

    public float smoothSpeed = 0.125f; // Ajusta la velocidad de seguimiento.

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}
