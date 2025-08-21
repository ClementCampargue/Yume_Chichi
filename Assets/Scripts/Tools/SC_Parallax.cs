using UnityEngine;

public class SC_Parallax : MonoBehaviour
{
    public Vector2 parallaxMultiplier = new Vector2(0.5f, 0.5f);
    public float smoothSpeed = 5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        targetPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0f);

        targetPosition += parallaxMovement;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        lastCameraPosition = cameraTransform.position;
    }
}