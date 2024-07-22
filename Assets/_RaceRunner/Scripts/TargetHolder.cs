using UnityEngine;

public class TargetHolder : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        target = CarController.Instance.transform;
    }

    private void Update()
    {
        Vector3 position = target.position;
        position.x = 0f;
        transform.position = position + offset;
    }
}