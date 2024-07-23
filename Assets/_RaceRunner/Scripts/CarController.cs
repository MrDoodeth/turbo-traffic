using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Ride")]
    public float maxSpeed;
    public const float MIN_SPEED = 7f;
    public float controlSensitivity;
    public float acceleration;
    public float braking;

    private const float MIN_X_BORDER = -6.764f;
    private const float MAX_X_BORDER = 6.764f;
    private const float ONCOMING_LANE_SHIFT = -0.5416915f;
    
    public int scoreOfOvertaking = 100;
    private const float OVERTAKING_SPEED = 26.3f;

    private const float SPEED_RATIO = 2.86f;
    private const float DISTANCE_RATIO = 1.2f;
    private const float SENSITIVITY_RATIO = 1f;
    private const float ACCELERATION_RATIO = 1f;
    private const float BRAKING_RATIO = 0.5f;

    public int GetSpeed() => (int)(currentSpeed * SPEED_RATIO);
    public int GetSensitivity() => (int)(controlSensitivity * SENSITIVITY_RATIO);
    public float GetAcceleration() => (int)(acceleration * ACCELERATION_RATIO);
    public float GetBraking() => (int)(braking * BRAKING_RATIO);
    public float GetDistance() => transform.position.z * DISTANCE_RATIO * 0.001f;

    [Space] 
    [Header("Animations")] 
    [SerializeField] private Transform Wheels;
    [SerializeField] private float angleBySpeed = 65f;
    [Space]
    
    [SerializeField] private Transform body;
    [SerializeField, Range(0.01f, 60f)] private float shiftHorizontalAngle;
    [SerializeField, Range(0.01f, 60f)] private float speedVerticalAngle;
    [SerializeField] private float bodyTime;
    [Space] 
    
    [SerializeField] private Transform car;
    [SerializeField, Range(0.01f, 60f)] private float rotateAngle;
    [SerializeField] private float carTime;
    public static bool isOnBorder;
    public static bool isOncoming;
    
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private Transform[] wheels;
    
    private Quaternion targetShift;
    private Quaternion targetRotation;
    private Quaternion targetSpeedRotation;
    
    private const float DEFAULT_BRAKING = 1f;
    private const float ACCIDENT_BRAKING = 55f;
    
    public static float currentSpeed;
    public static CarController Instance;
    public static bool isControllable = true;

    public DefaultInputActions controller;
    private Vector2 inputVector2;

    private void Awake()
    {
        controller = new DefaultInputActions();
        Wheels = transform.GetChild(1);
        body = transform.GetChild(0);
        body.AddComponent<GpuInstancerEnabler>();
        car = this.transform;
        
        rb = GetComponent<Rigidbody>();
        meshRenderer = body.GetComponent<MeshRenderer>();
        wheels = Wheels.GetComponentsInChildren<Transform>();
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
        isOnBorder = false;
        isOncoming = false;
    }

    private void Start()
    {
        isControllable = true;
        currentSpeed = MIN_SPEED;
        isOnBorder = false;
        isOncoming = false;
    }

    private void Update()
    {
        //Input
        var targetInputVector2 = isControllable ? controller.Player.Move.ReadValue<Vector2>() : Vector2.zero;
        inputVector2 = Vector2.Lerp(inputVector2, targetInputVector2, controlSensitivity * Time.deltaTime);
        var horizontal = inputVector2.x;
        var vertical = inputVector2.y;
        
        isOnBorder = transform.position.x is <= MIN_X_BORDER or >= MAX_X_BORDER;
        isOncoming = transform.position.x <= ONCOMING_LANE_SHIFT;
        
        #region Movement
        // Обновляем скорость с учетом ускорения/торможения
        if (vertical > 0.15f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (vertical < -0.15f)
        {
            currentSpeed -= braking * Time.deltaTime;
        }
        else
        {
            currentSpeed -= DEFAULT_BRAKING * Time.deltaTime;
        }

        if (isOnBorder)
            currentSpeed -= braking * Time.deltaTime;
        
        if (!isControllable)
            currentSpeed -= ACCIDENT_BRAKING * Time.deltaTime;
        

        // Ограничиваем скорость до minSpeed и maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, isControllable ? MIN_SPEED : 0f, maxSpeed);

        // Обновляем позицию машины
        Vector3 moveDirection =
            (transform.forward * currentSpeed) + (transform.right * (horizontal * controlSensitivity));
        
        
        rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MIN_X_BORDER, MAX_X_BORDER), transform.position.y,
            transform.position.z);
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

            if (Mathf.Approximately(currentSpeed, MIN_SPEED) || Mathf.Approximately(currentSpeed, maxSpeed))
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

        GameUIInfo.OnDistanceUpdated?.Invoke();
        GameUIInfo.OnSpeedUpdated?.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && isControllable)
        {
            //Finish
            isControllable = false;
            other.gameObject.GetComponent<EnemyCar>().enabled = false;
            other.transform.SetParent(transform, true);
            RaceManager.OnPlayerFinished?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (currentSpeed >= OVERTAKING_SPEED)
            RaceManager.OnOvertaking?.Invoke(scoreOfOvertaking);
    }

    public void ChangeMaterial(Material materialInstance)
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        meshRenderer.material = materialInstance;
    }
}