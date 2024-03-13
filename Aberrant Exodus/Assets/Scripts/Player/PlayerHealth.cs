using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public healthBar healthBar;

    public GameObject deathVFX;
    public LayerMask BossMask;
    public float playerHealth = 200f;
    public GameObject idle;
    bool touchedJombie , touchedSpider;
    public bool playerOn;
    bool availableDamage;
    public LayerMask spiderMask;
    [Header("camera shake")]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;
    public Collider2D col;
    bool touchedBoss;

    [Header("Flash")]
    public SpriteRenderer spriteRenderer;

    private Material originalMaterial;

    private Coroutine flashRoutine;

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    [Space]
    public LayerMask jombieMask;

    public GameObject coinPickupVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile"))
        {
            TakeDamage(20f); 
            
        }
        
    }


    private void Start()
    {
        healthBar.SetMaxHealth(playerHealth);
        playerOn = true;
        availableDamage = true;

        originalMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        Damage();
    }
    public void TakeDamage(float damageAmount)
    {
        CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);

        Flash();

        playerHealth -= damageAmount;
        healthBar.SetHealth(playerHealth);

        if (playerHealth <= 0)
        {
            playerOn = false;

            idle.GetComponent<SpriteRenderer>().enabled = false;

            col.enabled = false;

            StartCoroutine(HandleDeath());

            Audio_Manager.instance.Play(AllString.DEATH_AUDIO);

            CinemachineShake.Instance.ShakeCamera(50, 0.2f);
        }
    }

    IEnumerator HandleDeath()
    {
        GameManager.instance.isGameOver = true;

        GameObject playerVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(playerVFX, 4f);

        yield return new WaitForSeconds(1.1f);

        TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);


        yield return new WaitForSeconds(0.7f);


        if (SceneManager.GetActiveScene().buildIndex == 3)
            SceneManager.LoadScene(3);
        else if (SceneManager.GetActiveScene().buildIndex == 4)
            SceneManager.LoadScene(4);
        else
        {
            Destroy(GameManager.instance.gameObject);
            SceneManager.LoadScene(1);
        }

    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
 
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }

    void Damage()
    {
        touchedJombie = Physics2D.OverlapCircle(transform.position, 1f, jombieMask, Mathf.Infinity * -1, Mathf.Infinity);
        touchedSpider = Physics2D.OverlapCircle(transform.position, 1f, spiderMask, Mathf.Infinity * -1, Mathf.Infinity);
        touchedBoss = Physics2D.OverlapCircle(transform.position, 1f, BossMask, Mathf.Infinity * -1, Mathf.Infinity);
        
        if ((touchedJombie || touchedSpider || touchedBoss)  && availableDamage)
        {
            if (touchedSpider)
            {
                StartCoroutine(TakeDamg(10f));
            }
            if (touchedJombie)
            {
                StartCoroutine(TakeDamg(5f));
            }
            if (touchedBoss)
            {
                StartCoroutine(TakeDamg(80f));
            }
            
        }
        
    }

    IEnumerator TakeDamg(float damage)
    {
        TakeDamage(damage);
        availableDamage = false;
        yield return new WaitForSeconds(1f);
        availableDamage = true;

    }
}
