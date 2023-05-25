using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{ 
   Vector3 destination;

   private void OnCollisionEnter(Collision col)
   {
      if (this.name=="portal1")
      {
         destination = GameObject.Find("portal2").transform.position;
      }
      else
      {
         destination = GameObject.Find("portal1").transform.position;
      }

      col.transform.position = destination-Vector3.forward *2;
      col.transform.Rotate(Vector3.up*180);
   }
}
