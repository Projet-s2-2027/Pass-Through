using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class MonsterController : MonoBehaviour
{
    //Composants
    Animator animator;
 
    //Actions possibles
 
    //Stand ou Idle = attendre
    const string STAND_STATE = "Stand";
 
    //Reçoit des dommages
    const string TAKE_DAMAGE_STATE = "Damage";
 
    //Est vaincu
    public const string DEFEATED_STATE = "Defeated";
 
 
 
    //Mémorise l'action actuelle
    public string currentAction;
 
    private void Awake()
    {
        //Au départ, la créature attend en restant debout
        currentAction = STAND_STATE;
 
        //Référence vers l'Animator
        animator = GetComponent<Animator>();
    }
 
    private void Update()
    {
        //Si la créature reçoit des dommages:
        //Elle ne peut rien faire d'autres.
        //Cela servira quand on améliorera ce script.
        if (currentAction == TAKE_DAMAGE_STATE)
        {
            TakingDamage();
            return;
        }
 
        //...sera complété plus tard
    }
 
    //
    private void Stand()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Stand"
        currentAction = STAND_STATE;
        //Le paramètre "Stand" de l'animator = true
        
    }
 
    public void TakeDamage()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Damage"
        currentAction = TAKE_DAMAGE_STATE;
        //Le paramètre "Damage" de l'animator = true
    }
 
    public void Defeated()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Defeated"  
        currentAction = DEFEATED_STATE;
        //Le paramètre "Defeated" de l'animator = true
    }
 
 
    //Permet de surveiller l'animation lorque l'on prend un dégât
    private void TakingDamage()
    {
 
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(TAKE_DAMAGE_STATE))
        {
            //Compte le temps de l'animation
            //normalizedTime : temps écoulé nomralisé (de 0 à 1).
            //Si normalizedTime = 0 => C'est le début.
            //Si normalizedTime = 0.5 => C'est la moitié.
            //Si normalizedTime = 1 => C'est la fin.
 
 
            float normalizedTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
 
 
            //Fin de l'animation
            if (normalizedTime > 1)
            {
                Stand();
            }
 
        }
 
    }
 
    //Réinitialise les paramètres de l'animator
    private void ResetAnimation()
    {
        animator.SetBool(STAND_STATE, false);
        animator.SetBool(TAKE_DAMAGE_STATE, false);
        animator.SetBool(DEFEATED_STATE, false);
    }
 
}