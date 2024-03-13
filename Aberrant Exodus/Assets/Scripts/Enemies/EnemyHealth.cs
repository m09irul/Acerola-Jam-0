using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject deathVXF;
    [SerializeField] GameObject bombVFX;
    [SerializeField] GameObject commonDamageVXF;
    [SerializeField] GameObject deathBloodStain;

    gunMove gunmv;
    public float enemyHealth = 30f;
    float maxHealth;
    bool damageGiven;

    float shakeIntensity = 3;
    float shakeTime = 0.1f;

    public healthBar healthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(AllString.BULLET))
        {
            Damage(10f);
        }
        if (collision.CompareTag(AllString.BOMB))
        {
            Instantiate(bombVFX, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            
            Damage(30f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(enemyHealth);
        damageGiven = false;

        gunmv = GameObject.FindGameObjectWithTag(AllString.PLAYER_TAG).GetComponent<PlayerHealth>().idle.GetComponent<gunMove>();

        maxHealth = enemyHealth;
    }

    private void Update()
    {
        sr.color = Color.Lerp(Color.white, Color.red, (maxHealth - enemyHealth) / maxHealth);

        if (gunmv.healtCardActive && !damageGiven)
        {
            Instantiate(commonDamageVXF, transform.position, Quaternion.identity);
 
            Damage(30f);

            damageGiven = true;
        }
        if (!gunmv.healtCardActive)
            damageGiven = false;
    }
    public void Damage(float damageAmount)
    {

        enemyHealth -= damageAmount;
        healthBar.SetHealth(enemyHealth);
        if (enemyHealth <= 0f)
        {
            GameManager.instance.enemyCount--;

            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);

            Instantiate(deathVXF, transform.position, Quaternion.identity);

            GameObject bloodStain = Instantiate(deathBloodStain, transform.position, Quaternion.identity);
            bloodStain.transform.localRotation = Quaternion.Euler(bloodStain.transform.localRotation.x, bloodStain.transform.localRotation.y,
                Random.Range(0, 360));

            Destroy(gameObject);
        }
        else
            CinemachineShake.Instance.ShakeCamera(shakeIntensity / 2.5f, shakeTime / 2.5f);
    }
}
