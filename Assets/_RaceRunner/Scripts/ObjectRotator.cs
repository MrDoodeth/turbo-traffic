using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float rotationX = mouseX * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, -rotationX, Space.World);
        }
    }
}