using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeManager : MonoBehaviour
{
    public GameObject dialougePanel; // 3 total

    int index = 0;

    private void Start()
    {
        if (PlayerPrefs.GetInt(AllString.DIALOUGE, 0) != 3)
        {
            Audio_Manager.instance.Play(AllString.DEATH_AUDIO);
        }
    }
    private void Update()
    {
        if(PlayerPrefs.GetInt(AllString.DIALOUGE, 0) != 3)
        {
            dialougePanel.SetActive(true);
            if(index < 3)
                dialougePanel.transform.GetChild(index).gameObject.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Audio_Manager.instance.Play(AllString.DEATH_AUDIO);

            index++;
            dialougePanel.transform.GetChild(index - 1).gameObject.SetActive(false);
        }
        if(index > 2)
        {
            PlayerPrefs.SetInt(AllString.DIALOUGE, 3);
            dialougePanel.SetActive(false);
        }
    }
}
