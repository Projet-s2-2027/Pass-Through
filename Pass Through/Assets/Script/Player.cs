using System;
using Mirror;
using UnityEngine;
using System.Collections;
using Unity.Properties;
[RequireComponent(typeof(PlayerSetup))]

public class Player : NetworkBehaviour
{
    

    
    private bool _isDead=false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }

    }
    [SerializeField]
    private float maxHealth = 100f;
    
    [SyncVar]
    private float currentHealth;

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }
    public int kills;
    public int deaths;
    
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnableOnStart;

    private bool firstSetup = true;
    

    public void Setup()
    {
        //changement de camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        
        
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupOnAllCLients();
    }

    [ClientRpc]
    private void RpcSetupOnAllCLients()
    {
        if (firstSetup)
        {
            wasEnableOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnableOnStart[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefault();
    }

    
    public void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnableOnStart[i];
        }
        //reactive le collider du joueur
        Collider col = GetComponent<Collider>();
        if (col!=null)
        {
            col.enabled = true;
        }
        
        

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        
        yield return new WaitForSeconds(0.1f);

        Setup();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(25,"Joueur");
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount, string sourceID)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name +" a maintenant :"+ currentHealth+" points de vies.");

        if (currentHealth<=0)
        {
            Die(sourceID);
        }
        
    }

    private void Die(string sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourceID);
        if (sourcePlayer!=null)
        {
            sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(transform.name, sourcePlayer.name);
        }
        
        
        deaths++;
        

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col!=null)
        {
            col.enabled = false;
        }
        
        
        //changement de camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
        
        
        Debug.Log(transform.name+"a été éliminé.");
        StartCoroutine(Respawn());
        
        
        
        
    }
}
