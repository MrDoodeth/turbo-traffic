using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 150f;
    [SerializeField] private float upShift = 0.6f ;
    [SerializeField] private float upSpeed = 2.5f;
    private float startPositionY;
    [HideInInspector] public float timeShift;
    [HideInInspector] public GemSpawner gemSpawner;
    [HideInInspector] public int poolIndex;
    private const float DESTROY_DISTANCE = 10f;
    private void Awake()
    {
        startPositionY = transform.position.y;
    }
    private void Update()
    {
        //Position
        transform.position = new Vector3(transform.position.x, startPositionY + Mathf.Abs(Mathf.Sin((Time.timeSinceLevelLoad + timeShift) * upSpeed) * upShift), transform.position.z);
        
        //Rotation
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        
        if (gameObject.activeSelf)
        {
            if (transform.position.z - CarController.Instance.transform.position.z < -DESTROY_DISTANCE)
            {
                gemSpawner.ReleaseGem(this, false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.activeSelf)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gemSpawner.ReleaseGem(this, true);
            }
        }
    }
}
