 using System;
using UnityEngine;
using Mirror;
 using Unity.VisualScripting;

 [RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : NetworkBehaviour
{
   //vitesse du joueur
   [SerializeField]
   private float speed = 3f;
   [SerializeField]
   private float mouseSensitivityX = 3f;
   [SerializeField]
   private float mouseSensitivityY = 3f;

   public Camera playerCamera;

   [SerializeField] private float thrusterForce = 1000f;
   
   [Header("Joint Options")]
   [SerializeField] private float jointSpring = 20f;
   [SerializeField] private float jointMaxForce = 50f;
   
   //récupère les scripts du player motor
   private PlayerMotor motor;
   private ConfigurableJoint joint;

   private void Start()
   {
      //constructeur du motor
      motor = GetComponent<PlayerMotor>();
      joint = GetComponent<ConfigurableJoint>();
      SetJointSettings(jointSpring);
      
      
      //Disable camera if we are not the host
      //if (!isLocalPlayer)
      //{
        // playerCamera.gameObject.SetActive(false);
      //}
   }

   private void Update()
   {
      //If we are not the main client, don't run this method
      if (!isLocalPlayer)
      {
         return;
      }
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
      //On calcule la rotation de la caméra en Vector3
      float xRot = Input.GetAxisRaw("Mouse Y");
      
      float cameraRotationX = xRot * mouseSensitivityY;

      motor.RotateCamera(cameraRotationX);

      //calcul de la force du jetpack/thruster
      Vector3 thrusterVelocity = Vector3.zero;
      if (Input.GetButton("Jump"))
      {
         thrusterVelocity = Vector3.up * thrusterForce;
         SetJointSettings(0f);
      }
      else
      {
         SetJointSettings(jointSpring);
      }

      //appliquer la force du jetpack
      motor.ApplyThruster(thrusterVelocity);

   }

   private void SetJointSettings(float _jointSpring)
   {
      joint.yDrive= new JointDrive{ positionSpring = _jointSpring,maximumForce = jointMaxForce};
   }
   
}
