using System.Collections.Generic;
using UnityEngine;

public class AttackNearEnemy : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    [SerializeField] private List<GameObject> enemiesToAttack;

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Entity");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
