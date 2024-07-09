using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private void Update()
    {
        Vector3 position = target.position;
        position.x = 0f;
        transform.position = position + offset;
    }
}