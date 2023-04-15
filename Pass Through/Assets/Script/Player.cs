using System;
using Mirror;
using UnityEngine;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;
    private bool _isDead=false;
    
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnableOnStart;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }

    }

    public void Setup()
    {
        wasEnableOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnableOnStart[i] = disableOnDeath[i].enabled;
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

        Collider col = GetComponent<Collider>();
        if (col!=null)
        {
            col.enabled = true;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        SetDefault();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name +" a maintenant :"+ currentHealth+" points de vies.");

        if (currentHealth<=0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col!=null)
        {
            col.enabled = false;
        }
        Debug.Log(transform.name+"a été éliminé.");
        StartCoroutine(Respawn());
    }
}
