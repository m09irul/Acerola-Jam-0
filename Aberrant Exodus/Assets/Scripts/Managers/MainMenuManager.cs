using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    Audio_Manager audio_Manager;
    TransitionManager transition_Manager;
    private void Start()
    {
        audio_Manager = Audio_Manager.instance;
        transition_Manager = TransitionManager.instance;
        audio_Manager.Play(AllString.MAIN_MENU_AUDIO);

        transition_Manager.PlayANimLoad(AllString.CLOSE_TRANSITION);
    }
    public void PlayButtonPressed()
    {
        audio_Manager.Play(AllString.BUTTON_AUDIO);


        transition_Manager.PlayANimLoad(AllString.TRANSITION);

        Invoke("Load", 0.7f);
    }

    public void CreditButtonPressed()
    {
        audio_Manager.Play(AllString.BUTTON_AUDIO);

    }
    public void ExitButtonPressed()
    {
        audio_Manager.Play(AllString.BUTTON_AUDIO);

        Application.Quit();
    }

    void Load()
    {
        SceneManager.LoadScene(1);
    }
}
