using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    public string characterName;

    [Header("Speed")]
    [Range(100, 1000)]
    public float maxSpeed = 800f;
    [Tooltip("Smooth acceleration value")][Range(1, 100)]
    public float accelerationMomentum = 40f;
    [Range(1, 100)]
    public float deccelerationMomentum = 30f;
    [Range(1, 100)]
    public float turningMomentum = 25f;
    [Tooltip("At what speed player will start to change the movement direction. Defaulted at 80")]
    public float turningSpeedTreshold = 80f;

    [Header("Jump & Gravity")]
    [Range(0, 1000)]
    public float jumpForce = 600f;
    [Tooltip("Amount of force added when the player moving down. Defaulted at 80")][Range(0, 1000)]
    public float fallForce = 80f;
    [Tooltip("The maximum duration (in seconds) jump button can be held to jump higher")]
    public float jumpPressLimit = 0.2f;
}