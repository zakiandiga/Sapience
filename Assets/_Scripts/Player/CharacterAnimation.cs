using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationHolder animationHolder;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMove(float moveFloat)
    {
        if (Mathf.Abs(moveFloat) > 0.01f)
        {
            animator.SetFloat(animationHolder.runningFloat, moveFloat);
        }
        else
            animator.SetFloat(animationHolder.runningFloat, 0);
    }

    public void SetJump()
    {
        animator.Play(animationHolder.jumpTrigger, -1, 0);
    }

    ///Add another animation functions here
    ///Add the corresponding name (animation parameter or name) on the AnimationHolder.cs
    ///Update the AnimationHolder scriptable object's data 
}
