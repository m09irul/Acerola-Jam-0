using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunMove : MonoBehaviour
{
    public float movespeed = 5f, bulletspeed = 30f;
    Rigidbody2D rb;
    public Camera cam;
    public Transform firePos;
    public Transform firePos1;
    public Transform firePos2;
    public GameObject bullet;
    public GameObject granade;
    Vector2 movement;
    Vector2 mousePos;
    public bool threeBulletCard;
    public bool bombActive;
    public bool healtCardActive;
    public GameObject idle;
    public GameObject enemyType1;
    public GameObject enemyType3;
    public GameObject enemyType2;

    public PlayerHealth playerHealth;

    public GameObject shootVFX;
    public GameObject bombVFX;

    public GameObject[] cardColloectionVFX; // blue (bomb), yellow (enemy 20%), pink (three bullet)..

    public bool firedown;
    bool delayAvaiable;
    Coroutine cardRoutine;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bombActive = false;
        bombActive = false;
        healtCardActive = false;
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == AllString.END_TOGGLE)
        {
            SceneToggleManager.instance.EndToggle();
        }
        if (collision.CompareTag(AllString.THREE_BULLET))
        {
            Audio_Manager.instance.Play(AllString.KEY_AUDIO);
            Instantiate(cardColloectionVFX[2], collision.transform.position, Quaternion.identity);
            Destroy(collision.transform.parent.gameObject);


            if (cardRoutine != null)
                ResetFiring();

            threeBulletCard = true;
            cardRoutine = StartCoroutine(MakeFalse(10));
        }
        if (collision.CompareTag(AllString.BOMB_CARD))
        {
            Audio_Manager.instance.Play(AllString.KEY_AUDIO);
            Instantiate(cardColloectionVFX[0], collision.transform.position, Quaternion.identity);
            Destroy(collision.transform.parent.gameObject);

            if (cardRoutine != null)
                ResetFiring();

            bombActive = true;
            cardRoutine = StartCoroutine(MakeFalse(10));
        }
        if (collision.CompareTag(AllString.ENEMY_HEALTH))
        {
            Audio_Manager.instance.Play(AllString.KEY_AUDIO);
            Audio_Manager.instance.Play(AllString.BLAST_AUDIO);

            Instantiate(cardColloectionVFX[1], collision.transform.position, Quaternion.identity);
            Destroy(collision.transform.parent.gameObject);

            healtCardActive = true;
            StartCoroutine(MakeFalse(2));
        }
    }

    IEnumerator MakeFalse(int time)
    {
        yield return new WaitForSeconds(time);
        threeBulletCard = false;
        bombActive = false;
        healtCardActive = false;

    }
    void ResetFiring()
    {
        StopCoroutine(cardRoutine);
        threeBulletCard = false;
        bombActive = false;
        healtCardActive = false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = idle.transform.position;
        
        if (playerHealth.playerOn)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (!bombActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    firedown = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    firedown = false;
                }
                if (firedown && !delayAvaiable)
                {
                    StartCoroutine(Spray());
                }
            }
            else
            {
                if (Input.GetButtonDown(AllString.FIRE_BUTTON))
                {
                    BombFire();
                }
            }
        }


    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x)*Mathf.Rad2Deg ;
        rb.rotation = angle;
    }


    IEnumerator Spray()
    {
        Fire();
        delayAvaiable = true;
        yield return new WaitForSeconds(.1f);
        delayAvaiable = false;
    }
    void Fire()
    {
        if (threeBulletCard)
        {
            GameObject firedBullet1 = Instantiate(bullet, firePos1.transform.position, Quaternion.identity);
            firedBullet1.transform.rotation = firePos.transform.rotation;
            Rigidbody2D rb1 = firedBullet1.GetComponent<Rigidbody2D>();
            rb1.AddForce(firePos.right * bulletspeed, ForceMode2D.Impulse);

            GameObject firedBullet2 = Instantiate(bullet, firePos2.transform.position, Quaternion.identity);
            firedBullet2.transform.rotation = firePos.transform.rotation;
            Rigidbody2D rb2 = firedBullet2.GetComponent<Rigidbody2D>();
            rb2.AddForce(firePos.right * bulletspeed, ForceMode2D.Impulse);

            Destroy(firedBullet1, 5f);
            Destroy(firedBullet2, 5f);
        }
        Audio_Manager.instance.Play(AllString.BULLET_AUDIO);
        Instantiate(shootVFX, firePos.transform.position, Quaternion.identity);
        //GameObject firedBullet = poolobject.Instance.SpawnFromPool("Bullet", firePos.position, firePos.rotation);
        GameObject firedBullet = Instantiate(bullet, firePos.transform.position, Quaternion.identity);
        firedBullet.transform.rotation = firePos.transform.rotation;
        Rigidbody2D rb = firedBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.right * bulletspeed, ForceMode2D.Impulse);
        Destroy(firedBullet, 5f);
    }

    void BombFire()
    {
        Instantiate(bombVFX, firePos.transform.position, Quaternion.identity);
        Audio_Manager.instance.Play(AllString.BOMB_AUDIO);
        GameObject firedBullet = Instantiate(granade, firePos.transform.position, Quaternion.identity);
        firedBullet.transform.rotation = firePos.transform.rotation;
        Rigidbody2D rb = firedBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.right * 10, ForceMode2D.Impulse);
    }
}
