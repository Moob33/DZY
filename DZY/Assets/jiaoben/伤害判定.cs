using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 伤害判定 : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool player;
    [SerializeField] private bool enemy;
    private DamageType damageType;
    // Start is called before the first frame update
    void Start()
    {
}

    // Update is called once per frame
    void Update()
    {
        
    }
         private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                DamageData damageData = new DamageData(damage, damageType, gameObject);
                enemyHealth.TakeDamage(damageData);
            }
        }
    }
}
