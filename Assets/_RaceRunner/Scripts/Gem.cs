using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 150f;
    [SerializeField] private float upShift = 0.6f ;
    [SerializeField] private float upSpeed = 2.5f;
    [Space] 
    
    [SerializeField] private ParticleSystem gemVFXprefab;
    private float startPositionY;
    [HideInInspector] public float timeShift;
    [HideInInspector] public GemSpawner gemSpawner;
    [HideInInspector] public int poolIndex;
    private const float DESTROY_DISTANCE = 10f;
    private ParticleSystem currentGemVFX;

    private void Awake()
    {
        currentGemVFX = Instantiate(gemVFXprefab);
    }

    private void Start()
    {
        startPositionY = transform.position.y;
    }
    private void Update()
    {
        //Position
        transform.position = new Vector3(transform.position.x, startPositionY + Mathf.Abs(Mathf.Sin((Time.timeSinceLevelLoad + timeShift) * upSpeed) * upShift), transform.position.z);
        
        //Rotation
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        
        if (gameObject.activeSelf && CarController.isControllable)
        {
            if (transform.position.z - CarController.Instance.transform.position.z < -DESTROY_DISTANCE)
            {
                gemSpawner.ReleaseGem(this, false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.activeSelf && !other.collider.isTrigger)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gemSpawner.ReleaseGem(this, true);
                currentGemVFX.transform.position = transform.position;
                currentGemVFX.Play();
            }
            else
            {
                gemSpawner.ReleaseGem(this, false);
                currentGemVFX.transform.position = transform.position;
                currentGemVFX.Play();
            }
        }
    }
}
