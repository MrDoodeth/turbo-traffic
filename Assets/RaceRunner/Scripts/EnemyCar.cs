using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
