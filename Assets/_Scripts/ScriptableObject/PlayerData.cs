using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    public string characterName;

    [Header("Speed")]
    //[Range(0, 50)]
    public float groundSpeed;

    [Range(0, 50)]
    public float airSpeed;

    [Header("Jump & Gravity")]
    //[Range(0, 20)]
    public float jumpStrength;
    public float gravityValue;


}