
using UnityEngine;

[CreateAssetMenu( fileName = "WeaponData",menuName = "My Game/Weapon Data")]
public class weaponData:ScriptableObject
{
    public string name = "Assault Rifle";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;

    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public GameObject graphics;
}