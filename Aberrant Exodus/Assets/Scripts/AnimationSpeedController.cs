using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    Animator animator;
    public float animationSpeed = 1f;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
    }

}
