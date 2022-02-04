using UnityEngine;

[CreateAssetMenu(fileName = "newAnimationsHolder", menuName = "Data/PlayerData/AnimationHolder")]
public class AnimationHolder : ScriptableObject
{
    public string animationOwner;

    [Header("Movement")]
    [Tooltip("string of jumping animation STATE NAME")]
    public string jumpTrigger;
    [Tooltip("string of landing animation STATE NAME")]
    public string landTrigger;
    [Tooltip("string of falling animation PARAMETER NAME")]
    public string fallFloat;
    [Tooltip("string of running animation PARAMETER NAME")]
    public string runningFloat;

    [Header("Expressions")]
    [Tooltip("string of expression animations parameter name")]
    public string surprised;
    public string idea;
    public string exclamation;

}
