using UnityEngine;
using Mirror;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private Animator playerAnimator;
    private bool isrotated;

    [SerializeField]
    private GameObject playerGraphics;
    
    private weaponData currentWeapon;
    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            Debug.Log("Pas de camera renseigner");
            this.enabled = false;
        }

        isrotated = false;
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {currentWeapon = weaponManager.GetCurrentWeapon();
        if (PauseMenu.isOn)
        {
            CancelInvoke("Shoot");
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && weaponManager.currentMagazineSize < currentWeapon.magazineSize)
        {
            CancelInvoke("Shoot");
            StartCoroutine(weaponManager.Reload());
            return;
        }

        if (currentWeapon.fireRate<=0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CmdOnShootGraphic(true);
                Shoot();
            }
            else{
                CmdOnShootGraphic(false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CmdOnShootGraphic(true);
                InvokeRepeating("Shoot",0f,1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
                {
                    CmdOnShootGraphic(false);
                    CancelInvoke("Shoot");
                }
               
                
        }
        if (Input.GetButtonDown("Fire2") ){
            CmdOnAim(true);
        }
        if (Input.GetButtonUp("Fire2") ){
            CmdOnAim(false);
        }
    }

    [Command]
    void CmdOnShootGraphic(bool info){
        if ( info){
            RpcOnShootGraphic();
        }
        else{
            RpcOffShootGraphic();
        }
    }

    [ClientRpc]
    void RpcOnShootGraphic(){
        if (!isrotated){
                    playerGraphics.transform.Rotate(new Vector3(0f,52f,0f));    
                    isrotated = true;   
                     
                }
        playerAnimator.SetFloat("isShooting",1f);          
    }

    [ClientRpc]
    void RpcOffShootGraphic(){
        if(isrotated){
                        playerGraphics.transform.Rotate(new Vector3(0f,-52f,0f));    
                        isrotated = false;   
                        playerAnimator.SetFloat("isShooting",0f);
                    }
    }


    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal){
        RpcDoHitEffect(pos, normal);
    }

    [Command]
    void CmdOnAim(bool info){
        if ( info){RpcOnAim();}
        else{RpcOffAim();}
    }

    [ClientRpc]
    void RpcOnAim(){
        weaponManager.isAiming = true;
            playerAnimator.SetFloat("isAiming",1f);
            if (!isrotated){
                    playerGraphics.transform.Rotate(new Vector3(0f,48f,0f));    
                    isrotated = true;    
                }
    }

    [ClientRpc]
    void RpcOffAim(){
        weaponManager.isAiming = false;
            playerAnimator.SetFloat("isAiming",0f);
            if (isrotated){
                    playerGraphics.transform.Rotate(new Vector3(0f,-48f,0f));    
                    isrotated = false;    
                }
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal){
        GameObject hiteffecttemp = Instantiate(weaponManager.GetCurrentGraphics().hitEffect, pos, Quaternion.LookRotation(normal));
        Destroy(hiteffecttemp, 2f);
    }

    [Command]
    void CmdOnShoot(){
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect(){
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(currentWeapon.shootSound);
    }

    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer || weaponManager.isReloading)
        {return;}

        if (weaponManager.currentMagazineSize <= 0)
        {
           StartCoroutine(weaponManager.Reload());
            return;
        }

        weaponManager.currentMagazineSize--;
        RaycastHit hit;

        CmdOnShoot();

        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,currentWeapon.range, mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage,transform.name);
            }

            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage, string sourceID)
    {
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage,sourceID);
    }

}
