using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport1 : MonoBehaviour
{ 
   Vector3 destination;

   private void OnCollisionEnter(Collision col)
   {
      if (this.name=="portal3")
      {
         destination = GameObject.Find("portal4").transform.position;
      }
      else
      {
         destination = GameObject.Find("portal3").transform.position;
      }

      col.transform.position = destination+Vector3.forward ;
      col.transform.Rotate(Vector3.up*180);
   }
}
