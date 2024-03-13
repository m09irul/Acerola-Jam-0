using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
	[HideInInspector] public float horizontal;		
	[HideInInspector] public bool jumpHeld;			
	[HideInInspector] public bool jumpPressed;		

	public GameObject playerDeathVFX;
	public GameObject coinPickupVFX;

	bool readyToClear;                              

	[SerializeField] float shakeIntensity;
	[SerializeField] float shakeTime;

	private void Start()
	{
		TransitionManager.instance.PlayANimLoad(AllString.CLOSE_TRANSITION);
		Audio_Manager.instance.Play(AllString.BG_AUDIO);
	}
	void Update()
	{
		ClearInput();

		if (PlayerPrefs.GetInt(AllString.DIALOUGE, 0) != 3)
			return;

		if (GameManager.IsGameOver())
			return;
		if (GameManager.IsSwitchingScene())
			return;

		ProcessInputs();

		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}

	void FixedUpdate()
	{
		readyToClear = true;
	}

	void ClearInput()
	{
		if (!readyToClear)
		{
			return;
		}
        horizontal		= 0f;
		jumpPressed		= false;
		jumpHeld		= false;

		readyToClear	= false;
	}

	void ProcessInputs()
	{
		horizontal		+= Input.GetAxis(AllString.HORIZONTAL);

		jumpPressed		= jumpPressed || Input.GetButtonDown(AllString.JUMP);
		jumpHeld		= jumpHeld || Input.GetButton(AllString.JUMP);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == AllString.START_TOGGLE)
        {
			SceneToggleManager.instance.StartToggle();
			collision.enabled = false;
        }
        if (collision.tag == AllString.END_TOGGLE)
        {
            SceneToggleManager.instance.EndToggle();
			collision.enabled = false;
        }
        if (collision.tag == AllString.CHECKPOINT)
        {
            GameManager.instance.normalPlayerInitialPosition = transform.position;

        }
        if (collision.tag == AllString.NORMAL_KEY)
		{
			Audio_Manager.instance.Play(AllString.KEY_AUDIO);

			Destroy(collision.transform.parent.gameObject);

			Instantiate(coinPickupVFX, collision.transform.position, Quaternion.identity);

			GameManager.instance.normalKeyCount++;

			GameManager.instance.normalKeyPositions.Add(collision.gameObject.transform.parent.position);

			UIManager.instance.UpdateNormalKeyUI(14 - GameManager.instance.normalKeyCount);

		}

		if (collision.tag == AllString.FINAL_KEY)
		{
            //open gateway..
            DoorManager.instance.OpenDoor();

            Audio_Manager.instance.Play(AllString.KEY_AUDIO);

			Destroy(collision.gameObject);

			Instantiate(coinPickupVFX, collision.transform.position, Quaternion.identity);

		}

		if (collision.tag == AllString.OBSATACLE && !GameManager.instance.isSheildActivated)
		{
			Audio_Manager.instance.Play(AllString.DEATH_AUDIO);

			StartCoroutine(HandleDeath());

			CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
		}

		if (collision.tag == AllString.FINAL_CHAOTIC_TAG)
		{
			TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);

			Invoke("LoadFinalChaoticScene", 0.7f);

		}
        if (collision.tag == AllString.END_GAME)
        {
			StartCoroutine(FinishGame());
        }
    }
    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1f);

        TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);

        yield return new WaitForSeconds(0.7f);

        //destroy existing game manager..

        SceneManager.LoadScene(5);

        Destroy(GameManager.instance.gameObject);
    }
    IEnumerator HandleDeath()
	{
		GameManager.instance.isGameOver = true;

		GameObject playerVFX = Instantiate(playerDeathVFX, transform.position, Quaternion.identity);
		Destroy(playerVFX, 4f);
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<Collider2D>().enabled = false;
		yield return new WaitForSeconds(1.1f);

		TransitionManager.instance.PlayANimLoad(AllString.TRANSITION);

		yield return new WaitForSeconds(0.7f);
		//as this script will only remain on normal level so this will reload normal level only.. 
		LoadCurrentScene();

	}

	void LoadFinalChaoticScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
	}

	void LoadCurrentScene()
	{
        GameManager.instance.isGameOver = false;
        //Destroy(GameManager.instance.gameObject);
        if (SceneManager.GetActiveScene().buildIndex != 4)
			GameManager.instance.OnBegin();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
