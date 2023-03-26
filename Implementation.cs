using Il2Cpp;
using MelonLoader;
using Mono.Cecil;
using UnityEngine;
using GearSpawner;
using System.Reflection;
using MoreLockedDoors.GearSpawns;

namespace MoreLockedDoors;
internal sealed class Implementation : MelonMod
{

    public override void OnInitializeMelon()
	{
		MelonLogger.Msg("More Locked Doors is online.");
		ItemSpawnManager.InitializeCustomHandler();
		ItemSpawnManager.InitializeItemSpawns();
	}

}
