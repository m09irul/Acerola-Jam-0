using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class EnemyBossAI : MonoBehaviour
{
    public Animator[] animator;
    public GameObject missile;
    public Transform shootPos;
    public Transform shootPos1;
    public Transform shootPos2;
    public float bossHeath = 1000f;
    float maxHealth;
    bool attacked;
    public int bossState = 1;
    Vector2 bossPlace;
    GameObject player;
    gunMove gunmv;
    public GameObject setSpawner;
    public GameObject setSpawner2;
    bool shootEnabled = true;
    public healthBar healthBar;
    AIDestinationSetter destin;
    bool damageGiven;

    [SerializeField] GameObject deathVXF;
    [SerializeField] GameObject bombVFX;
    [SerializeField] GameObject commonDamageVXF;
    [SerializeField] GameObject duringEnemySpawnBossVFX;

    float shakeIntensity = 5;
    float shakeTime = 0.15f;

    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = bossHeath;
        healthBar.SetMaxHealth(bossHeath);
        bossPlace = transform.position;
        setSpawner.transform.position = bossPlace;
        player = GameObject.FindGameObjectWithTag(AllString.PLAYER_TAG);
        gunmv = player.GetComponent<PlayerHealth>().idle.GetComponent<gunMove>();
        destin = GetComponent<AIDestinationSetter>();
        destin.target = player.transform;
        attacked = false;
        damageGiven = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(AllString.PLAYER_TAG))
        {
            // play attack animation.............................................................................................................
            destin.enabled = false;
        }
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
    int c1 = 0;
    int c2 = 0;
    int c3 = 0;
    int d = 0;
    GameObject vfx;
    // Update is called once per frame
    void Update()
    {
        bossPlace = transform.position;
        if (gunmv.healtCardActive && !damageGiven)
        {
            Instantiate(commonDamageVXF, transform.position, Quaternion.identity);

            Damage(30f);

            damageGiven = true;
        }
        if (!gunmv.healtCardActive)
            damageGiven = false;

        if (bossState == 1)
        {


            if (c1 == 0)
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].gameObject.SetActive(false);
                }
                animator[0].gameObject.SetActive(true);
                sr = animator[0].gameObject.GetComponent<SpriteRenderer>();
                c1 = 1;
            }

            if (!attacked)
            {
                // show moving animation...........................................................................................................

                animator[0].Play(AllString.ENEMY_MOVE_ANIM);
                

                StartCoroutine(AttackNow());
            }
        }
        if(bossState == 2)
        {

            if (c2 == 0)
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].gameObject.SetActive(false);
                }
                animator[1].gameObject.SetActive(true);
                sr = animator[0].gameObject.GetComponent<SpriteRenderer>();
                c2 = 1;
            }

            if (d == 0)
            {
                vfx = Instantiate(duringEnemySpawnBossVFX, shootPos.position, Quaternion.identity);
                d = 1;
            }

            vfx.transform.position = transform.position;

            if (transform.position.x == bossPlace.x && transform.position.y == bossPlace.y)
            {
                // play enemy spawn vfx;...........................................................................................................
                animator[1].Play(AllString.ENEMY_IDLE_ANIM);

            }
            else
            {
                //Show moving animation...........................................................................................................
                animator[1].Play(AllString.ENEMY_MOVE_ANIM);

            }

            destin.enabled = false;
            transform.position = Vector2.MoveTowards(transform.position , bossPlace , 1f * Time.fixedDeltaTime);
            BossRotation();
            setSpawner.SetActive(true);
            setSpawner2.SetActive(true);
        }
        if(bossState == 3)
        {
            Destroy(vfx);
            if (c3 == 0)
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].gameObject.SetActive(false);
                }
                animator[2].gameObject.SetActive(true);
                sr = animator[2].gameObject.GetComponent<SpriteRenderer>();
                c3 = 1;
            }

            BossRotation();
            setSpawner.SetActive(false);
            setSpawner2.SetActive(false);
            if (shootEnabled)
            {
                //show shooting animation...........................................................................................................
                animator[1].Play(AllString.ENEMY_ATTACK_ANIM);
                StartCoroutine(Shoot());
            }
            else
            {
                //show idle animation...........................................................................................................
                animator[1].Play(AllString.ENEMY_IDLE_ANIM);
            }
        }

        sr.color = Color.Lerp(Color.white, Color.red, (maxHealth - bossHeath) / maxHealth);
    }

    IEnumerator Shoot()
    {
        shootEnabled = false;
        Instantiate(missile, shootPos.position, Quaternion.identity);
        Instantiate(missile, shootPos1.position, Quaternion.identity);
        Instantiate(missile, shootPos2.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        shootEnabled = true;
    }

    IEnumerator AttackNow()
    {
        destin.enabled = true;
        attacked = true;

        yield return new WaitForSeconds(1f);

        destin.enabled = false;

        yield return new WaitForSeconds(1f);
        
        attacked = false;
    }

    void Damage(float damageAmount)
    {
        bossHeath -= damageAmount;
        healthBar.SetHealth(bossHeath);
        if(bossHeath <= 0)
        {
            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);

            bossState += 1;
            if (bossState <= 3)
            {
                bossHeath = 1000f;
                healthBar.SetMaxHealth(bossHeath);
            }
            else
            {
                //playDestroyAnimation
                //Time.timeScale = .2f;
                StartCoroutine(DestroyafterAnimaiton());
            }
        }
        else
            CinemachineShake.Instance.ShakeCamera(shakeIntensity / 2.5f, shakeTime / 2.5f);
    }

    IEnumerator DestroyafterAnimaiton()
    {
        Instantiate(deathVXF, transform.position, Quaternion.identity);
        Audio_Manager.instance.Play(AllString.BOSS_DESTROY_AUDIO);
        Audio_Manager.instance.Stop(AllString.FINAL_CHAOS_AUDIO);

        yield return new WaitForSeconds(0.5f);
        GameManager.instance.LoadEscapeScene();
        Destroy(gameObject);
    }

    void BossRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, RotaitonofZ(gameObject.transform.position));

        if (transform.position.x > player.transform.position.x)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector2(1, -1);
        }
    }

    float RotaitonofZ(Vector2 position)
    {
        float x, y;
        x = player.transform.position.x - position.x;
        y = player.transform.position.y - position.y;

        float rotation = Mathf.Atan(y / x);

        rotation *= 180 / Mathf.PI;

        rotation += 90;

        return rotation;
    }
}
