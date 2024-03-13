using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        TransitionManager.instance.PlayANimLoad(AllString.CLOSE_TRANSITION);

        yield return new WaitForSeconds(6f);

        TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);

        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(0);
    }

   
}
