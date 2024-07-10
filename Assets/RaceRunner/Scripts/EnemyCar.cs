using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float speed;
    private Transform player;
    private EnemyCarSpawner manager;
    private const float DESTROY_DISTANCE = 10f;
    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        if (gameObject.activeSelf)
        {
            if (transform.position.z - player.transform.position.z < -DESTROY_DISTANCE)
            {
                manager.ReleaseEnemyCar(this);
            }
        }
    }

    public void SetSpawnParameters(Transform player, EnemyCarSpawner manager)
    {
        this.player = player;
        this.manager = manager;
    }
}
