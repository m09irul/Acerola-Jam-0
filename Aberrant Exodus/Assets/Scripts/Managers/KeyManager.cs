using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public GameObject[] normalKeys;
    public GameObject finalKey;

    public GameObject normalPlayer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < normalKeys.Length; i++)
        {
            for (int j = 0; j < GameManager.instance.normalKeyPositions.Count; j++)
            {

                if (normalKeys[i].transform.position == GameManager.instance.normalKeyPositions[j])
                {
                    normalKeys[i].SetActive(false);
                    
                }

            }
            
        }

        normalPlayer.transform.position = GameManager.instance.normalPlayerPosition;

    }

}
