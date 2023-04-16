using Il2Cpp;
using MelonLoader;
using Mono.Cecil;
using UnityEngine;
using GearSpawner;
using System.Reflection;
using MoreLockedDoors.GearSpawns;
using MoreLockedDoors.Utils;

namespace MoreLockedDoors;
internal sealed class Implementation : MelonMod
{

    internal static SaveDataManager sdm = new SaveDataManager();
    public override void OnInitializeMelon()
	{
		MelonLogger.Msg("More Locked Doors is online.");
		ItemSpawnManager.InitializeCustomHandler();
	}

}
