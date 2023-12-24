using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreLockedDoors.GearSpawns.CustomHelperClasses
{
    internal class QuaternionSer
    {

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        public QuaternionSer(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w; 
        }

        public QuaternionSer()
        {
        }


    }
}
