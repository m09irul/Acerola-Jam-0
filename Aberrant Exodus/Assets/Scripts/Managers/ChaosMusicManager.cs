using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosMusicManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        int i = Random.Range(9, 14);
        //9-13 chaos music...
        Audio_Manager.instance.sounds[i].source.Play();
    }


}
