using UnityEngine;

public class FaceMovementDirection : MonoBehaviour
{
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 movement = transform.position - lastPosition;

        // On ne tourne que s'il y a un d�placement significatif
        if (movement.sqrMagnitude > 0.0001f)
        {
            // D�termine la nouvelle direction
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);

            // Applique la rotation (interpolation pour plus de fluidit�)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        lastPosition = transform.position;
    }
}

