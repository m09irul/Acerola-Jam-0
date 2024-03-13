using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class M_Platform : MonoBehaviour
{
    Vector2 startPoint;
    [SerializeField] Vector2 endPoint;

    [SerializeField] bool hasTriggered = false;
    [SerializeField] public float movementDuration;
    private void Start()
    {
        startPoint = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(AllString.PLAYER_TAG) && !hasTriggered)
        {
            hasTriggered = true;
            LeanTween.move(gameObject, endPoint, movementDuration)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() =>
            {
                LeanTween.move(gameObject, startPoint, movementDuration)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() =>
            hasTriggered = false
            );
            });
        }
    }

}
