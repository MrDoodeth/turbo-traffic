using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float controlSensitivity;
    [SerializeField] private float acceleration;
    [SerializeField] private float braking;
    private Rigidbody rb;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = minSpeed;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * controlSensitivity;
        var vertical = Input.GetAxis("Vertical");
        

        // Обновляем скорость с учетом ускорения/торможения
        if (vertical > 0f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (vertical < 0f)
        {
            currentSpeed -= acceleration * braking * Time.deltaTime;
        }

        // Ограничиваем скорость до minSpeed и maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Обновляем позицию машины
        Vector3 moveDirection = (transform.forward * currentSpeed) + (transform.right * horizontal);

        rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);
    }
}