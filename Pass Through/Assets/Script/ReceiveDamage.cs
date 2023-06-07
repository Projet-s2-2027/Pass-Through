using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    public int maxHitPoint = 5;
    public int HitPoint = 0;
    public float maxHealth;
    public float currentHealth;
    public bool isInvulnerable;
    public float invulnerabilityTime;
    private float timeSinceLastHit;

    void Start(){
        HitPoint = maxHitPoint;
        currentHealth = maxHealth;
    }

    void Update(){
        if (isInvulnerable){
            timeSinceLastHit += Time.deltaTime;

            if (timeSinceLastHit > invulnerabilityTime){
                timeSinceLastHit = 0f;
                isInvulnerable =false;
            }
        }
    }

    public void GetDamage(float Damage){
        if (isInvulnerable){
            return;
        }

        if (currentHealth <= 0f){
            if (HitPoint <= 0f){
                gameObject.SendMessage("Defeated", SendMessageOptions.DontRequireReceiver);
            }
            else{
                currentHealth = maxHealth;
                HitPoint--;
            }
        }

        else{
            currentHealth -= Damage;
            gameObject.SendMessage("TakeDamage",SendMessageOptions.DontRequireReceiver);
        }
    }

}
