using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Util
{
    public class JointFinder : SerializedMonoBehaviour
    {
       [SerializeField,ReadOnly] private Dictionary<Joint, Transform> _joints = new Dictionary<Joint, Transform>();


       private void Awake()
       {
           _joints.Clear();

           var children = GetComponentsInChildren<Transform>(true);
           var jointNames = (Joint[])Enum.GetValues(typeof(Joint));

           foreach (var child in children)
           {
               if (child.CompareTag("Untagged"))
                   continue;

               foreach (var jointName in jointNames)
               {
                   if (child.CompareTag(jointName.ToString()))
                   {
                       if (!_joints.ContainsKey(jointName))
                       {
                           _joints.Add(jointName, child);
                       }
                       else
                       {
                           Debug.LogWarning($"Duplicate joint {jointName} found on {child.name}, skipping...");
                       }
                   }
               }
           }
       }

       public Transform FindJoint(Joint rightHand)
       {
           if(!_joints.ContainsKey(rightHand))
               Debug.LogError("WE DONT HAVE THIS JOINT MOTHER FUCKER BITCH BOI");
           return _joints[rightHand];
       }
    }
}
