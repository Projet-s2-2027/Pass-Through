using UnityEngine;
using Mirror;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    
    private PlayerWeapon currentWeapon;
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
    {
        if (PauseMenu.isOn)
        {
            return;
        }
        currentWeapon = weaponManager.GetCurrentWeapon();

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

    [Client]
    private void Shoot()
    {
        Debug.Log("Tir effectué");
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,currentWeapon.range, mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage)
    {
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }
//test pour commit
}
