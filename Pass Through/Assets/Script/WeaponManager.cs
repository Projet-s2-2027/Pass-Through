using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;
    
    private PlayerWeapon currentWeapon;
    
    [SerializeField] 
    private Transform weaponHolder;
    
    [SerializeField]
    private string weaponLayerName = "Weapon";

    
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns =Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns,LayerMask.NameToLayer(weaponLayerName));
        }
    }
    
}

