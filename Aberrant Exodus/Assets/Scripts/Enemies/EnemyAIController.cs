using Pathfinding;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public Animator anim;
    AIPath aiPath;          //aidestination setter script
    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag(AllString.PLAYER_TAG).transform;
    }

    private void Update()
    {
        if (aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
        {
            // play attack Animation;
            anim.Play(AllString.ATTACK_ANIM);
        }
        else
        {
            // play running animation;
            anim.Play(AllString.WALKING_ENEMY_ANIM);
        }
    }

}
