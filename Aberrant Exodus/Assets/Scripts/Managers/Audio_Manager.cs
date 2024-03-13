using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Audio_Manager : MonoBehaviour
{
    public Sounds[] sounds;

    public AudioClip gameOverClip;

    public static Audio_Manager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.masterMixtureGroup;
            s.source.pitch = s.pinch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s==null)
        {
            Debug.LogWarning("sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound: " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            Stop(AllString.MAIN_MENU_AUDIO);

        if (SceneManager.GetActiveScene().buildIndex != 1)
            Stop(AllString.BG_AUDIO);

        //9-13 chaos music...
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            for (int i = 9; i < 14; i++)
            {
                sounds[i].source.Stop();
            }
        }

        if (SceneManager.GetActiveScene().buildIndex != 3 && SceneManager.GetActiveScene().buildIndex != 4)
            Stop(AllString.FINAL_CHAOS_AUDIO);


    }

}
