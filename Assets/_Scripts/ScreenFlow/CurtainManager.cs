using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void FinishFadeTo()
    {
        OnFinishFadeTo?.Invoke(this);
    }

    private void FinishFadeFrom()
    {
        OnFinishFadeFrom?.Invoke(this);
    }
}
