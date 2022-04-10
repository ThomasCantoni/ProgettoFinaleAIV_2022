using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

   public static class GlobalVariables
   {
    
    /// <summary>
    /// THIS VECTOR3 IS NORMALIZED. DO NOT USE THIS IF PLAYER IS NOT GROUNDED
    /// </summary>
        public static Vector3 PlayerVelocityGrounded;
        public static Quaternion PlayerRotation;
    /// <summary>
    /// THIS VECTOR3 IS NOT NORMALIZED. USE THIS IF THE PLAYER IS NOT GROUNDED.
    /// </summary>
    public static Vector3 PlayerVelocityNotGrounded;
        public static bool IsPlayerGrounded;
    public  static  Vector3 PlayerVelocityAuto 
    { 
        get
        {
            if(IsPlayerGrounded)
            {
                return PlayerVelocityGrounded;
            }
            else
            {
                return PlayerVelocityNotGrounded;
            }
        }
     }
}

