using System;
using System.Collections.Generic;
using UnityEngine;

public class CurtainManager : MonoBehaviour
{
    public static event Action<CurtainManager> OnFinishFadeTo;
    public static event Action<CurtainManager> OnFinishFadeFrom;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        FadeFrom();
    }

    public void FadeTo()
    {
        animator.Play("FadeTo");
    }

    public void FadeFrom()
    {
        animator.Play("FadeFrom");
    }

    public void FinishFadeTo()
    {
        OnFinishFadeTo?.Invoke(this);
    }

    public void FinishFadeFrom()
    {
        OnFinishFadeFrom?.Invoke(this);
    }
}
