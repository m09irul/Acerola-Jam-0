using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControl : MonoBehaviour
{
    [SerializeField]
    private float speed = .5f;
    Rigidbody2D rb;
    bool slowDown;
    public bool blasted;
    // Start is called before the first frame update
    void Start()
    {
        slowDown = false;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Explosion());
    }
    ///IEnumerator
    // Update is called once per frame
    void Update()
    {
        if(!slowDown)
             transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag(AllString.BOMB))
            PlayAnimation();
    }



    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(.4f);

        PlayAnimation();
    }

    void PlayAnimation()
    {

        // Play animation;
        slowDown = true;
        blasted = true;
        StartCoroutine(VanishGranade());
    }

    IEnumerator VanishGranade()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
