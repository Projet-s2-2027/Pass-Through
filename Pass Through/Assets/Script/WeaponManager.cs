using System.Collections;
using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private weaponData primaryWeapon;
    
    private weaponData currentWeapon;
    private WeaponGraphics currentGraphics;
    
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

    public WeaponGraphics GetCurrentGraphics(){
        return currentGraphics;
    }

    void EquipWeapon(weaponData _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns =Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (currentGraphics == null){
            Debug.Log("pas de weapon");
        }

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

