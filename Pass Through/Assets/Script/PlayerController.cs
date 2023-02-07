using System;
using UnityEngine;

[RequireComponent(typeof(playerMotor))]
public class PlayerController : MonoBehaviour
{
   //vitesse du joueur
   [SerializeField]
   private float speed = 3f;
   [SerializeField]
   private float mouseSensitivityX = 3f;
   [SerializeField]
   private float mouseSensitivityY = 3f;
   
   //récupère les scripts du player motor
   private playerMotor motor;

   private void Start()
   {
      //constructeur du motor
      motor = GetComponent<playerMotor>();
   }

   private void Update()
   {
      //calcul de la vélocité du mouvement du joueur
      float xMov = Input.GetAxisRaw("Horizontal");
      float zMov = Input.GetAxisRaw("Vertical");

      Vector3 moveHorizontal = transform.right * xMov;
      Vector3 moveVertical = transform.forward * zMov;

      Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
      
	   motor.Move(velocity);
      
      //On calcule la rotation du joueur en Vector3
      float yRot = Input.GetAxisRaw("Mouse X");
      
      Vector3 rotation = new Vector3(0,yRot,0) * mouseSensitivityX;

      motor.Rotate(rotation);
      //On calcule la rotation du joueur en Vector3
      float xRot = Input.GetAxisRaw("Mouse Y");
      
      Vector3 cameraRotation = new Vector3(xRot,0,0) * mouseSensitivityY;

      motor.RotateCamera(cameraRotation);
      



   }
}
