using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "Scriptable Objects/WeaponUpgradeData")]
public class WeaponData : UpgradeData
{
    public Sprite chainSprite;
    public float velocity;
    public int limit;
    public float charge;
    public float size;

    public bool isChaining;
    public AudioClip shootSound;
}
