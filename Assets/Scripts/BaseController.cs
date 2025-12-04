using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyTypes;
    public float health = 100f;
    public float money = 0f;

    void Update()
    {
        if(health <= 0)
        {
            Debug.Log("Game Over!");
        }
    }

    public void DealDamageToBase(float damage)
    {
        health -= damage;
    }
}
