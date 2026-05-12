using System;
using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    Animator animator;
    string currentState = "Idle";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;

        currentState = newState;
        animator.Play(newState);
    }

}
