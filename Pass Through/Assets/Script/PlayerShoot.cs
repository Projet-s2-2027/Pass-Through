using UnityEngine;
using Mirror;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    
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

        
        weaponManager = GetComponent<WeaponManager>();

    }

    private void Update()
    {currentWeapon = weaponManager.GetCurrentWeapon();
        if (PauseMenu.isOn)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(weaponManager.Reload());
            return;
        }

        if (currentWeapon.fireRate<=0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot",0f,1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal){
        RpcDoHitEffect(pos, normal);
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
