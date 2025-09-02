using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectA.Data.Types
{
    [Serializable]
    public class StrikeData
    {
        [SerializeField]public int ID;
        [SerializeField]public float StrikeTime;
        [SerializeField]public bool HasStruck;
        [SerializeField]public Transform StrikeLocation;

       
    }
}