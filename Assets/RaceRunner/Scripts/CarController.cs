using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    [Header("Ride")] 
    public float maxSpeed;
    public float minSpeed;
    [SerializeField] private float controlSensitivity;
    [SerializeField] private float acceleration;
    [SerializeField] private float braking;

    [Space] 
    [Header("Animations")] 
    [SerializeField] private Transform Wheels;
    [SerializeField] private float angleBySpeed = 85f;
    [Space] 
    
    [SerializeField] private Transform body;
    [SerializeField, Range(0.01f, 60f)] private float shiftHorizontalAngle;
    [SerializeField, Range(0.01f, 60f)] private float speedVerticalAngle;
    [SerializeField] private float bodyTime;
    [Space] 
    
    [SerializeField] private Transform car;
    [SerializeField, Range(0.01f, 60f)] private float rotateAngle;
    [SerializeField] private float carTime;
    [HideInInspector] public bool isOnBorder;
    private Rigidbody rb;
    public float currentSpeed { get; private set; }
    private Transform[] wheels;
    private Quaternion targetShift;
    private Quaternion targetRotation;
    private Quaternion targetSpeedRotation;
    private const float DEFAULT_BRAKING = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheels = Wheels.GetComponentsInChildren<Transform>();
        currentSpeed = minSpeed;
    }

    private void Update()
    {
        #region Movement

        //Input
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");


        // Обновляем скорость с учетом ускорения/торможения
        if (vertical > 0f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (vertical < 0f)
        {
            currentSpeed -= braking * Time.deltaTime;
        }
        else
        {
            currentSpeed -= DEFAULT_BRAKING * Time.deltaTime;
        }

        if (isOnBorder)
            currentSpeed -= braking * Time.deltaTime;

        // Ограничиваем скорость до minSpeed и maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Обновляем позицию машины
        Vector3 moveDirection =
            (transform.forward * currentSpeed) + (transform.right * (horizontal * controlSensitivity));

        rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);

        #endregion

        #region Animations

        //Wheels
        for (var i = 1; i < wheels.Length; i++)
        {
            wheels[i].Rotate(Vector3.right, angleBySpeed * currentSpeed * Time.deltaTime);
        }

        if (!isOnBorder)
        {
            targetShift = Quaternion.Euler(0f, 0f, horizontal * shiftHorizontalAngle);
            targetRotation = Quaternion.Euler(0f, horizontal * rotateAngle, 0f);

            if (Mathf.Approximately(currentSpeed, minSpeed) || Mathf.Approximately(currentSpeed, maxSpeed))
                targetSpeedRotation = Quaternion.identity;
            else
                targetSpeedRotation = Quaternion.Euler(vertical * -speedVerticalAngle, 0f, 0f);
        }
        else
        {
            targetShift = Quaternion.identity;
            targetRotation = Quaternion.identity;
            targetSpeedRotation = Quaternion.identity;
        }

        body.localRotation = Quaternion.Lerp(body.localRotation, targetShift * targetSpeedRotation,
            bodyTime * Time.deltaTime);
        car.localRotation = Quaternion.Lerp(car.localRotation, targetRotation, carTime * Time.deltaTime);

        #endregion
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(0);
        }
    }
}