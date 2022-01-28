using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    public string dataOwner;

    [Header("Ground Speed")]
    [Range(0, 50)]
    public float groundSpeed = 15f;
    [Range(0, 50)]
    public float accelTime = 20f;
    [Range(0, 50)]
    public float deccelTime = 18.4f;

    [Header("Air Speed")]
    [Range(0, 50)]
    public float airSpeed = 8f;
    [Range(0, 50)]
    public float airAccelTime = 5f;

    [Header("Jump & Gravity")]
    [Range(0, 20)]
    public float jumpHeight = 8;
    public float jumpConst = -1f;
    public float gravityValue = -98.5f;
    public float airAttackGravityMod = -10f;
    public int maxJumpCount = 2;
    public float recoveryFall = 0.5f;
    public float recoveryAirNormalAttack = 0.8f;

    public float staggerTime = 0.8f;

}