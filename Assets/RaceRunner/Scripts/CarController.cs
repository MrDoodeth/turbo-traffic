using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Ride")] [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float controlSensitivity;
    [SerializeField] private float acceleration;
    [SerializeField] private float braking;

    [Space] 
    [Header("Animations")] 
    [SerializeField] private Transform Wheels;
    [SerializeField] private float angleBySpeed = 85f;
    [Space] 
    
    [SerializeField] private Transform body;
    [SerializeField, Range(0.01f, 5f)] private float shiftAngle;
    [SerializeField] private float shiftTime;
    [Space] 
    
    [SerializeField] private Transform car;
    [SerializeField, Range(0.01f, 5f)] private float rotateAngle;
    [SerializeField] private float rotateTime;
    private Rigidbody rb;
    private float currentSpeed;
    private Transform[] wheels;
    private Quaternion targetShift;
    private Quaternion targetRotation;

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
        var horizontal = Input.GetAxis("Horizontal") * controlSensitivity;
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

        // Ограничиваем скорость до minSpeed и maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Обновляем позицию машины
        Vector3 moveDirection = (transform.forward * currentSpeed) + (transform.right * horizontal);

        rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);

        #endregion

        #region Animations

        //Wheels
        for (var i = 1; i < wheels.Length; i++)
        {
            wheels[i].Rotate(Vector3.right, angleBySpeed * currentSpeed * Time.deltaTime);
        }
        
        //Body
        targetShift = Quaternion.Euler(0f, 0f, horizontal * shiftAngle);
        body.localRotation = Quaternion.Lerp(body.localRotation, targetShift, shiftTime * Time.deltaTime);
        
        //Car
        targetRotation = Quaternion.Euler(0f, horizontal * rotateAngle, 0f);
        car.localRotation = Quaternion.Lerp(car.localRotation, targetRotation, rotateTime * Time.deltaTime);
        #endregion
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            currentSpeed -= braking * Time.deltaTime;
            targetShift = Quaternion.identity;
            targetRotation = Quaternion.identity;
        }
            
    }
}