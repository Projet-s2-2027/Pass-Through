 using System;
 using System.Numerics;
 using UnityEngine;
using Mirror;

 using Vector3 = UnityEngine.Vector3;

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
   
   [SerializeField]
   private float thrusterFuelBurnSpeed = 1f;
   [SerializeField] 
   private float thrusterFuelRegenSpeed = 0.3f;
   private float thrusterFuelAmount = 1f;

   public float GetThrusterFuelAmount()
   {
      return thrusterFuelAmount;
   }
   
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
      if (PauseMenu.isOn)
      {
         if (Cursor.lockState!=CursorLockMode.None)
         {
            Cursor.lockState = CursorLockMode.None;
         }
         motor.Move(Vector3.zero);
         motor.Rotate(Vector3.zero);
         motor.RotateCamera(0f);
         motor.ApplyThruster(Vector3.zero);
         
         return;
      }

      if (Cursor.lockState != CursorLockMode.Locked)
      {
         Cursor.lockState = CursorLockMode.Locked;
      }

      RaycastHit _hit;
      if (Physics.Raycast(transform.position,Vector3.down, out _hit,100f))
      {
         joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
      }
      else
      {
         joint.targetPosition = new Vector3(0f, 0f, 0f);
      }
      
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
      if (Input.GetButton("Jump") && thrusterFuelAmount>0)
      {
         thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

         if (thrusterFuelAmount>=0.01f)
         {
            thrusterVelocity = Vector3.up * thrusterForce;
            SetJointSettings(0f);
         }
         
      }
      else
      {
         thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
         SetJointSettings(jointSpring);
      }
      thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

      //appliquer la force du jetpack
      motor.ApplyThruster(thrusterVelocity);

   }

   private void SetJointSettings(float _jointSpring)
   {
      joint.yDrive= new JointDrive{ positionSpring = _jointSpring,maximumForce = jointMaxForce};
   }
   
}
