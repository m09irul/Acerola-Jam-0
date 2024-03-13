using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderDuplication : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject deathVXF;
    [SerializeField] GameObject bombVFX;

    [SerializeField]public healthBar healthBar;
    public GameObject SpiderSec1;
    public float enemyHealth = 20f;
    float maxHealth;

    float shakeIntensity = 3;
    float shakeTime = 0.1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(AllString.BULLET))
        {
            Damage(10);
        }
        if (collision.CompareTag(AllString.BOMB))
        {
            Instantiate(bombVFX, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Damage(30);
        }
    }
    private void Start()
    {
        healthBar.SetMaxHealth(enemyHealth);
        maxHealth = enemyHealth;
    }
    private void Update()
    {
        sr.color = Color.Lerp(Color.white, Color.red, (maxHealth - enemyHealth) / maxHealth);

    }
    void Damage(float amount)
    {
        
        enemyHealth -= amount;
        healthBar.SetHealth(enemyHealth);
        if(enemyHealth <= 0)
        {
            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
            Instantiate(deathVXF, transform.position, Quaternion.identity);
            DamageAndDuplicate();
        }
        else
            CinemachineShake.Instance.ShakeCamera(shakeIntensity / 2.5f, shakeTime / 2.5f);
    }

    void DamageAndDuplicate()
    {
        

        Instantiate(SpiderSec1, new Vector3( transform.position.x , transform.position.y + 1f, 0), Quaternion.identity);
        Instantiate(SpiderSec1, new Vector3(transform.position.x, transform.position.y - 1f, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
