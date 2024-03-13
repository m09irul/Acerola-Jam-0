using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyAIMissile : MonoBehaviour
{
    public GameObject missile;
    public Transform shootPos;
    AIPath aiPath;          
    bool callActive ;

    public Animator anim;

    private void Start()
    {
        callActive = true;
        aiPath = GetComponent<AIPath>();
        GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag(AllString.PLAYER_TAG).transform;
    }
    // Update is called once per frame
    void Update()
    {
        //TakeDamage();
        if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
        {
            // play idle animation;
            anim.Play(AllString.ATTACK_ANIM);
            TakeDamage();
        }
        else
        {
            anim.Play(AllString.WALKING_ENEMY_ANIM);
            //play running animation;
        }
    }
    void TakeDamage()
    {
        if (callActive)
        {
            StartCoroutine(FireMissile());
        }
    }

    IEnumerator FireMissile()
    {
        Instantiate(missile, shootPos.position, Quaternion.identity);
        callActive = false;
        yield return new WaitForSeconds(1f);
        callActive = true;
    }
}
