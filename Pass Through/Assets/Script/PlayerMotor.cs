using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   
   private Vector3 _velocity;
   private Vector3 _rotation;
   private float cameraRotationX=0f;
   private float currentCameraRotationX = 0f;
   public float jumpPower = 4.5f;
   private Vector3 thrusterVelocity;
   [SerializeField] private float cameraRotationLimit = 85f;

   private Rigidbody rb;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   public void Move(Vector3 velocity)
   {
      this._velocity = velocity;
   }
   
   public void Rotate(Vector3 rotation)
   {
      this._rotation = rotation;
   }
   public void RotateCamera(float cameraRotationX)
   {
      this.cameraRotationX = cameraRotationX;
   }

   public void ApplyThruster(Vector3 _thrusterVelocity)
   {
      thrusterVelocity = _thrusterVelocity;
   }
   //applique le mouvement au Rigidbody.
   private void FixedUpdate()
   {
      PerformMovement();
      PerformRotation();
      if (Input.GetKeyDown(KeyCode.Space))
      {
         Jump();
      }
   }

   private void PerformMovement()
   {
      if (_velocity != Vector3.zero)
      {
         rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
      }

      if (thrusterVelocity != Vector3.zero)
      {
         rb.AddForce(thrusterVelocity*Time.fixedDeltaTime,ForceMode.Acceleration);
      }
   }

   private void PerformRotation()
   {
      rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
      currentCameraRotationX -= cameraRotationX;
      currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
      cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

   }

   private void Jump()
   {
      rb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
   }
   
}
