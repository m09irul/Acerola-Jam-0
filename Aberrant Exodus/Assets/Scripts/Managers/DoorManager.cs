using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
	public static DoorManager instance;

	public GameObject DoorOpenVFX;
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

	}

	public void OpenDoor()
	{
		GameObject vfx =  Instantiate(DoorOpenVFX, transform.position, Quaternion.identity);
		Destroy(vfx, 5f);

		gameObject.SetActive(false);
	}
}
