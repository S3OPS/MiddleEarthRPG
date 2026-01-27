using UnityEngine;

public class SimpleCameraLook : MonoBehaviour
{
    public Transform Target;
    public float Sensitivity = 1.8f;

    private float _pitch;

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        var mouseX = Input.GetAxis("Mouse X") * Sensitivity;
        var mouseY = Input.GetAxis("Mouse Y") * Sensitivity;

        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -75f, 75f);

        Target.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}