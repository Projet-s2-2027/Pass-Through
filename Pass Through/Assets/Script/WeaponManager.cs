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
    private PlayerController playerController;
    
    [SerializeField] 
    private Transform WeaponHolderAim;
    
    [SerializeField] 
    private Camera cam;
    [SerializeField] 
    public Transform WeaponHolder;
    private bool hasSecondary;

    public bool isAiming;

    public bool isReloading = false;

    public bool isCamLimit;

    [SerializeField]
    private Animator PlayerAnimator;
    
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [HideInInspector] 
    public int currentMagazineSize;

    
    void Start()
    {
        EquipWeapon(primaryWeapon);
        isAiming = false;
        playerController = GetComponent<PlayerController>();
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
        isAiming = false;
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
                PlayerAnimator.SetBool("hasSecondary",false);
                hasSecondary = false;
            }
        }
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.B))
        {
            if ( currentWeapon != secondaryWeapon && secondaryWeapon != null)
            {
                Destroy(GetCurrentGraphics().gameObject);
                EquipWeapon(secondaryWeapon);
                PlayerAnimator.SetBool("hasSecondary",true);
                hasSecondary = true;
            }
        }
        if (isAiming && isLocalPlayer){
            playerController.currentSpeed = playerController.GetdefaultSpeed()*0.6f;
            currentGraphics.transform.SetParent(WeaponHolderAim);
        }
        else if (isLocalPlayer){
            playerController.currentSpeed = playerController.GetdefaultSpeed();
            currentGraphics.transform.SetParent(WeaponHolder);
        }
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

