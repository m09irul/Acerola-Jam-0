using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
	public static TransitionManager instance;


	public Animator switchSceneTransitionAnimator;
    public Animator loadSceneTransitionAnimator;
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(gameObject);
	}

    public void PlayAnimSwitch(string name)
	{
        Audio_Manager.instance.Play(AllString.LOAD_TRANSITION);
        Audio_Manager.instance.Play(AllString.SWITCH_TRANSITION);
		switchSceneTransitionAnimator.Play(name);
	}

	public void PlayANimLoad(string name)
	{
        Audio_Manager.instance.Play(AllString.LOAD_TRANSITION);
		loadSceneTransitionAnimator.Play(name);
	}
}
