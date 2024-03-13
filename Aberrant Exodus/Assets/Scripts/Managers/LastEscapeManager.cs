using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEscapeManager : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        GameManager.instance.isGameOver = false;
        Audio_Manager.instance.Play(AllString.FINAL_CHAOS_AUDIO);
        //Audio_Manager.instance.Stop(AllString.TIMER_AUDIO);

        player.transform.localPosition = Vector3.zero;

        CinemachineShake.Instance.ShakeCamera(2.5f, 999999f);
    }
}
