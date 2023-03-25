using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    private void Awake()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(""+transform.name +""+ currentHealth);
    }
}
