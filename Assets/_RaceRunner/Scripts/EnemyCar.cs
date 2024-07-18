using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float speed;
    private Transform player;
    private EnemyCarSpawner manager;
    private const float DESTROY_DISTANCE = 10f;
    public void SetSpawnParameters(Transform player, EnemyCarSpawner manager)
    {
        this.player = player;
        this.manager = manager;
    }
    private void Update()
    {
        if (!CarController.Instance.isControllable)
            return;
        
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        if (gameObject.activeSelf)
        {
            if (transform.position.z - player.transform.position.z < -DESTROY_DISTANCE)
            {
                manager.ReleaseEnemyCar(this);
            }
        }
    }
    //
    // private void OnCollisionStay(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         manager.ReleaseEnemyCar(this);
    //     }
    // }
}
