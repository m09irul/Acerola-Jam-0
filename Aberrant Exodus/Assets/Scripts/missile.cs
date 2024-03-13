using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    float speed = 6f, lifeTime = 4f;            // bullet speed and life time
    GameObject player;                           // to access player;
    bool Collided;

    public GameObject missileBlastVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collided = false;

        //vfx
        Instantiate(missileBlastVFX, transform.position, Quaternion.identity);

        VanishBullet();

    }
    // Start is called before the first frame update
    void Start()
    {
        Collided = false;
        player = GameObject.FindWithTag(AllString.PLAYER_TAG);
        StartCoroutine(CountDownTimer());       //to destroy as soon as animation ends
    }
    // Update is called once per frame
    void Update()
    {
        if(!Collided)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(player.transform.position == transform.position)
        {
            VanishBullet();
        }

        MissileRotation();
    }
    //to destroy as soon as animation ends
    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        VanishBullet();
    }
    //to destroy as soon as animation ends
    void VanishBullet()
    {
        Destroy(gameObject);
    }


    void MissileRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, RotaitonofZ(gameObject.transform.position));

        if (transform.position.x > player.transform.position.x)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector2(1, 1);
        } 
    }

    float RotaitonofZ(Vector2 position)
    {
        float x, y;
        x = player.transform.position.x - position.x;
        y = player.transform.position.y - position.y;

        float rotation = Mathf.Atan(y / x);

        rotation *= 180 / Mathf.PI;

        return rotation;
    }
}
