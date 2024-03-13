using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.Rendering.Universal;
using Pathfinding;
using UnityEngine.Profiling;

public class SceneToggleManager : MonoBehaviour
{
    public GameObject grid1, grid2, player1, player2, startToggleObj;
    public CinemachineVirtualCamera VirtualCamera;

    public static SceneToggleManager instance;
    public Volume vol;
    public ChromaticAberration chromaticAberration;
    public LensDistortion lensDistortion;

    Coroutine cr;
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
        vol.profile.TryGet<LensDistortion>(out lensDistortion);

        vol.profile.TryGet<ChromaticAberration>(out chromaticAberration);
    }
    private void ToggleScene()
    {
        if (GameManager.instance.isGameOver)
            return;

        Audio_Manager.instance.Play(AllString.FLICKER_AUDIO);
        chromaticAberration.active = !chromaticAberration.IsActive();

        float val;
        if (Random.Range(0, 1f) > 0.5f)
            val = 0.6f;
        else 
            val = -0.6f;

        LeanTween.value(gameObject, -0.1f, val, 0.1f).setOnUpdate((float value) => lensDistortion.intensity.Override(value)).
            setOnComplete(() => {
                chromaticAberration.active = !chromaticAberration.IsActive();
                LeanTween.value(gameObject, val, -0.1f, 0.1f).setOnUpdate((float value) => lensDistortion.intensity.Override(value));
            });

        grid1.SetActive(!grid1.activeSelf);
        player1.SetActive(!player1.activeSelf);

        grid2.SetActive(!grid2.activeSelf);
        player2.SetActive(!player2.activeSelf);

        if (grid1.activeSelf)
        { 
            player1.transform.position = new Vector2(player2.transform.GetChild(0).position.x, player2.transform.GetChild(0).position.y);
            VirtualCamera.Follow = player1.transform;
        }
        else 
        {
            player2.transform.GetChild(0).position = new Vector2(player1.transform.position.x, player1.transform.position.y);
            VirtualCamera.Follow = player2.transform.GetChild(0);
        }

    }

    IEnumerator RandomTimerCoroutine()
    {
        while (true)
        {
            float randomSeconds = Random.Range(1.1f, 2.3f);

            yield return new WaitForSeconds(randomSeconds);

            ToggleScene();
        }
    }

    public void StartToggle()
    {
        cr = StartCoroutine(RandomTimerCoroutine());

        startToggleObj.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void EndToggle()
    {
        StopCoroutine(cr);
        Audio_Manager.instance.Play(AllString.FLICKER_AUDIO);
        grid1.SetActive(true);
        player1.SetActive(true);

        grid2.SetActive(false);
        player2.SetActive(false);

        VirtualCamera.Follow = player1.transform;

        chromaticAberration.active = false;
        lensDistortion.intensity.Override(-0.1f);
    }
}
