using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class B_Platform : MonoBehaviour
{
    [SerializeField] GameObject secondPlatform;
    [SerializeField] float shakeX, shakeY, shakeDuration, alphaDuration;
    [SerializeField] bool hasTriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(AllString.PLAYER_TAG) && !hasTriggered)
        {
            hasTriggered = true;

            LeanTween.move(gameObject, gameObject.transform.position + new Vector3(shakeX, shakeY, 0), shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong(5)
            .setOnComplete(() =>
            {
                GetComponent<BoxCollider2D>().enabled = false;

                LeanTween.alpha(gameObject, 0f, alphaDuration)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    LeanTween.delayedCall(gameObject, 0.5f, () =>
                    {
                        LeanTween.rotate(secondPlatform, gameObject.transform.eulerAngles, 0.7f).setEase(LeanTweenType.easeInOutSine);
                        LeanTween.move(secondPlatform, gameObject.transform.position, 1f).setEase(LeanTweenType.easeInOutSine);
                    });
                });
            });
        }
    }
}