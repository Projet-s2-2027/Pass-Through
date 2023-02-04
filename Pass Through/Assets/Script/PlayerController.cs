using System;
using UnityEngine;

[RequireComponent(typeof(playerMotor))]
public class PlayerController : MonoBehaviour
{
   //vitesse du joueur
   [SerializeField]
   private float speed;
   
   //récupère les scripts du player motor
   private playerMotor _motor;

   private void Start()
   {
      //constructeur du motor
      _motor = GetComponent<playerMotor>();
   }

   private void Update()
   {
      //calcul de la vélocité du mouvement du joueur
      float xMov = Input.GetAxisRaw("Horizontal");
      float zMov = Input.GetAxisRaw("Vertical");

      Vector3 moveHorizontal = transform.right * xMov;
      Vector3 moveVertical = transform.forward * zMov;

      Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
   }
}
