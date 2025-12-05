using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            Destroy(gameObject);
            CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
            if(hud != null)
            {
                hud.AddCoins(coinOnDeath);
            }
        }
    }

}
