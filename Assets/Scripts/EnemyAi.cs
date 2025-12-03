using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, target.position, speed * Time.deltaTime);
    }
}
