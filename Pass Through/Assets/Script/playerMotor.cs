using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   
   private Vector3 _velocity;
   private Vector3 _rotation;
   private Vector3 _cameraRotation;
   public float jumpPower = 4.5f;

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
   public void RotateCamera(Vector3 cameraRotation)
   {
      this._cameraRotation = cameraRotation;
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
   }

   private void PerformRotation()
   {
      rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
      cam.transform.Rotate(-_cameraRotation);
   }

   private void Jump()
   {
      rb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
   }
   
}
