using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace MoreLockedDoors.Utils
{
    internal class Systems
    {

        public static GameObject GetEngineSystems()
        {
            return GameObject.Find("SCRIPT_EngineSystems");
        }

        public static bool IsAssemblyPresent(string assemblyName)
        {
            MelonAssembly assembly = MelonAssembly.LoadedAssemblies.FirstOrDefault(obj => obj.Assembly.GetName().Name.Contains(assemblyName));
            return (assembly != null);
        }

    }
}
