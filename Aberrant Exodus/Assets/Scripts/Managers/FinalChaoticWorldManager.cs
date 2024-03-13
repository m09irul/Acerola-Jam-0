using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChaoticWorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.isGameOver = false;

        TransitionManager.instance.PlayANimLoad(AllString.CLOSE_TRANSITION);

        Audio_Manager.instance.Play(AllString.FINAL_CHAOS_AUDIO);
        Audio_Manager.instance.Stop(AllString.TIMER_AUDIO);
    }


}
