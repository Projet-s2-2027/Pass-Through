using System;
using Mirror;
using UnityEngine;
using System.Collections;
using Unity.Properties;
[RequireComponent(typeof(PlayerSetup))]

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    public string username = "Player";

    public bool interacte;
    
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

    [SerializeField]
    private AudioClip deadSound;
    

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
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            CmdInteracteChange(true);
        }
        else
        {
            CmdInteracteChange(false);
        }
    }

    [Command]
    void CmdInteracteChange(bool info){
        RpcInteracteChange(info);
    }

    [ClientRpc]
    void RpcInteracteChange(bool info){
        interacte = true;
    }


    [ClientRpc]
    public void RpcTakeDamage(float amount, string sourceID)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;

        if (currentHealth<=0)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(deadSound);
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
            GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
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

        StartCoroutine(Respawn());
         
    }
}
