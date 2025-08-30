using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Helpers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Util
{
    public class JointFinder : SerializedMonoBehaviour
    {
       [OdinSerialize,ReadOnly] private Dictionary<Joint, JointHelper> _joints = new Dictionary<Joint, JointHelper>();


       private void Awake()
       {
           _joints = new Dictionary<Joint, JointHelper>();
           var children = GetComponentsInChildren<JointHelper>(true);
           
           foreach (var child in children)
           {
               if (!_joints.TryAdd(child.Joint, child))
               {
                   Debug.LogWarning($"Duplicate joint {child.Joint} found on {child.name}, skipping...");
               }
           }
       }

       public Transform FindJoint(Joint joint)
       {
           if(!_joints.ContainsKey(joint))
               Debug.LogError("WE DONT HAVE THIS JOINT MOTHER FUCKER BITCH BOI");
           return _joints[joint].transform;
       }
    }
}
