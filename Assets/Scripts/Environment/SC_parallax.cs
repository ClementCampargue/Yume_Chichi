using UnityEngine;

public class SC_parallax : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    public Vector2 intensity = new Vector2(0.5f, 0.5f);

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;


        Vector3 parallax = delta * intensity;
        transform.position += new Vector3(parallax.x, parallax.y, 0);

        lastCameraPosition = cameraTransform.position;
    }
}