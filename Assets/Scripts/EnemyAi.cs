using Unity.VisualScripting;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float health;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            target.GetComponent<BaseController>().DealDamageToBase(damageOnImpact);
            
            Destroy(gameObject);
            CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
            if(hud != null)
            {
                hud.AddCoins(coinOnDeath);
            }
            
        }
    }
}
