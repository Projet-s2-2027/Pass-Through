using System.Collections;
using UnityEngine;
using Mirror;


public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private weaponData primaryWeapon;
    [SerializeField]
    private weaponData secondaryWeapon;
    [SerializeField]
    private weaponData knife;
    
    private weaponData currentWeapon;
    private WeaponGraphics currentGraphics;
    
    [SerializeField] 
    private Transform leftHand;
    [SerializeField] 
    private Transform RightHand;
    [SerializeField] 
    private Transform WeaponHolder;

    public bool isReloading = false;

    [SerializeField]
    private Animator PlayerAnimator;
    
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [HideInInspector] 
    public int currentMagazineSize;

    
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
        currentMagazineSize = _weapon.magazineSize;

        GameObject weaponIns =Instantiate(_weapon.graphics);
        weaponIns.transform.SetParent(WeaponHolder);
        

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
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentMagazineSize = currentWeapon.magazineSize;
        isReloading = false;
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.V)) 
        {
            if (currentWeapon != primaryWeapon && primaryWeapon != null)
            {
                Destroy(GetCurrentGraphics().gameObject);
                EquipWeapon(primaryWeapon);
                PlayerAnimator.SetBool("hasPrimary",true);
                PlayerAnimator.SetBool("hasSecondary",false);
            }
        }
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.B))
        {
            if ( currentWeapon != secondaryWeapon && secondaryWeapon != null)
            {
                Destroy(GetCurrentGraphics().gameObject);
                EquipWeapon(secondaryWeapon);
                PlayerAnimator.SetBool("hasPrimary",false);
                PlayerAnimator.SetBool("hasSecondary",true);
            }
        }
        WeaponHolder.position = RightHand.position;
        WeaponHolder.LookAt(leftHand);
    }

    [Command]
    void CmdOnReload() 
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload() 
    { 
        Animator animator = currentGraphics.GetComponent<Animator>();
        if (animator != null) 
        {
            animator.SetTrigger("Reload");
        }
    }
}

