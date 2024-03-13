using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	public TextMeshProUGUI normalKeyText;			
	public TextMeshProUGUI finalKeyText;			

	public TextMeshProUGUI switchWorldTimerText;		

	public TextMeshProUGUI gameOverText;
    public CanvasGroup normalPlayershield;

    void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
	}

	private void Start()
	{
		if(normalKeyText != null)
			UpdateNormalKeyUI(14 - GameManager.instance.normalKeyCount);
		if (finalKeyText != null)
			UpdateFinalKeyUI(1 - GameManager.instance.FinalKeyCount);

	}
	public void UpdateNormalKeyUI(int keyCount)
	{
		if (instance == null)
			return;
        if (keyCount < 0)
            instance.normalKeyText.text = "0";
        else
            instance.normalKeyText.text = keyCount.ToString();
	}
	public void UpdateFinalKeyUI(int keyCount)
	{
		if (instance == null)
			return;
		if(keyCount<0)
			instance.finalKeyText.text = "0";
		else
			instance.finalKeyText.text = keyCount.ToString();
	}

	public void UpdateswitchWorldTimerUI(int time)
	{
		if (instance == null)
			return;

		instance.switchWorldTimerText.text = time.ToString();

		if (time < 6)
		{
			instance.switchWorldTimerText.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 1);
			instance.switchWorldTimerText.color = new Color(1, 0, 0, 1);
		}
		else
		{
			instance.switchWorldTimerText.transform.parent.GetComponent<Image>().color = new Color(0.9f, 1f, 0.95f, 1f);
			instance.switchWorldTimerText.color = new Color(0, 0, 0, 1);
		}
	}
	
	public void DisplayGameOverText()
	{
		if (instance == null)
			return;

		instance.gameOverText.enabled = true;
	}
}
