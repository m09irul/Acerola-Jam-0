using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

[DefaultExecutionOrder(-101)]
public class GameManager : MonoBehaviour
{

	public static GameManager instance;

	public bool isGameOver;                            
	public bool isSwitchingScene;                           
	public bool isSheildActivated = false;                           

	public int normalKeyCount = 0;
	public int FinalKeyCount = 0;

	public int totalKeys;
	public List<Vector3> normalKeyPositions = new List<Vector3>();

	public float goChaoticTimerDuration;
	float goChaoticTimer;
	bool goChaoticTimerIsRunning = true;

	public float goNormalTimerDuration;
	float goNormalTimer;
	bool goNormalTimerIsRunning = false;

	public Vector3 normalPlayerPosition;
	public Vector3 normalPlayerInitialPosition;

	public int enemyCount = 0;

	int c = 0;
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

	private void Start()
    {
		OnBegin();
    }

    public void OnBegin()
    {
		normalPlayerPosition = normalPlayerInitialPosition;

        c = 0;

        goChaoticTimer = goChaoticTimerDuration;
        goNormalTimer = goNormalTimerDuration;

        TransitionManager.instance.PlayANimLoad(AllString.CLOSE_TRANSITION);
    }

    IEnumerator HandleDeath()
	{
		TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);

		yield return new WaitForSeconds(0.7f);
		//as this script will only remain on normal level so this will reload normal level only.. 
		//destroy existing game manager..

		Destroy(gameObject, 0.1f);

		SceneManager.LoadScene(1);



	}
	void Update()
	{
		if (PlayerPrefs.GetInt(AllString.DIALOUGE, 0) != 3)
			return;

		if (isGameOver)
		{
			Audio_Manager.instance.Stop(AllString.TIMER_AUDIO);

			return;
		}

		if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
			return;

		if (goChaoticTimerIsRunning)
		{

			if (goChaoticTimer < 6 && goChaoticTimer > 0)
			{
				if (c == 0)
				{
					c = 1;
					Audio_Manager.instance.Play(AllString.TIMER_AUDIO);
				}
			}
			else
			{
				c = 0;
				Audio_Manager.instance.Stop(AllString.TIMER_AUDIO);
			}

			//Timer section...........
			if (goChaoticTimer > 0)
			{
				goChaoticTimer -= Time.deltaTime;

				UIManager.instance.UpdateswitchWorldTimerUI((int)goChaoticTimer);
			}
			else
			{
				goChaoticTimerIsRunning = false;
				goChaoticTimer = goChaoticTimerDuration;

				//save player position..
				normalPlayerPosition =  KeyManager.instance.normalPlayer.transform.position;

				//load chaotic scene..
				StartCoroutine(SwitchScene(true));
			}

			
		}

		else if (goNormalTimerIsRunning)
		{

			if (goNormalTimer < 6 && goNormalTimer > 0)
			{
				if (c == 0)
				{
					c = 1;
					Audio_Manager.instance.Play(AllString.TIMER_AUDIO);
				}
			}
			else
			{
				c = 0;
				Audio_Manager.instance.Stop(AllString.TIMER_AUDIO);
			}

			//Timer section...........
			if (goNormalTimer > 0)
			{
				goNormalTimer -= Time.deltaTime;

				UIManager.instance.UpdateswitchWorldTimerUI((int)goNormalTimer);
			}
			else
			{
				goNormalTimerIsRunning = false;
				goNormalTimer = goNormalTimerDuration;

				//load normal scene..
				StartCoroutine(SwitchScene(false));

			}

		}

		//check for keys..
		if (normalKeyCount == totalKeys - 1 && goChaoticTimerIsRunning)
		{
			//turn off all timer..
			goChaoticTimerIsRunning = false;
			goNormalTimerIsRunning = false;

			//open gateway..
			DoorManager.instance.OpenDoor();

			//load final chaotic world..
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
		}
	}

	IEnumerator SwitchScene(bool switchToChaotic)
	{
		if (!isGameOver)
		{
			EnableSheild();

			isSwitchingScene = true;
			//do transition anim..
			TransitionManager.instance.PlayAnimSwitch(AllString.TRANSITION);

			yield return new WaitForSeconds(2f);

			if (switchToChaotic)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

				TransitionManager.instance.PlayAnimSwitch(AllString.CLOSE_TRANSITION);

				isSwitchingScene = false;

				// activate chatic timer.. 
				goNormalTimerIsRunning = true;
			}
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

				TransitionManager.instance.PlayAnimSwitch(AllString.CLOSE_TRANSITION);

				isSwitchingScene = false;

				// activate chatic timer.. 
				goChaoticTimerIsRunning = true;

				DisableSheild();
			}
		}
	}
	public void LoadEscapeScene()
	{
		LeanTween.value(gameObject, 0, 1, 5f).setOnComplete(() =>

         StartCoroutine(SwitchScene(true))
		);
        
    }
    void EnableSheild()
    {
		if (UIManager.instance.normalPlayershield == null)
			return;

		LeanTween.value(gameObject, 0, 1, 0.2f).setOnUpdate(
			(float val) =>
			{
				UIManager.instance.normalPlayershield.alpha = val;
			});
		isSheildActivated = true;
    }
    void DisableSheild()
	{
		LeanTween.value(gameObject, 1, 0, 2.5f).setOnUpdate(
			(float val) =>
			{
                UIManager.instance.normalPlayershield.alpha = val;
            }).setOnComplete(()=>
                    isSheildActivated = false
			);
	}
	public static bool IsGameOver()
	{
		if (instance == null)
			return false;

		return instance.isGameOver;
	}

	public static bool IsSwitchingScene()
	{
		if (instance == null)
			return false;

		return instance.isSwitchingScene;
	}
}
