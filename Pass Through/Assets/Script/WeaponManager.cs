using System.Collections;
using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private weaponData primaryWeapon;
    
    private weaponData currentWeapon;
    
    [SerializeField] 
    private Transform weaponHolder;

    public bool isReloading = false;
    
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField] public int currentMagazineSize;

    
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public weaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(weaponData _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns =Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns,LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public IEnumerator Reload()
    {
        if (isReloading)
        {yield break;}

        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentMagazineSize = currentWeapon.magazineSize;
        isReloading = false;
    }
    
}

