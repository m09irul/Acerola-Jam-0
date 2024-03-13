using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletControl : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VanishBullet());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Jombie")) 
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
    }
    IEnumerator VanishBullet()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
