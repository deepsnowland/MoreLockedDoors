using Il2Cpp;
using MelonLoader;
using Mono.Cecil;
using UnityEngine;
using GearSpawner;
using System.Reflection;
using MoreLockedDoors.GearSpawns;
using MoreLockedDoors.Utils;
using MoreLockedDoors.Locks;

namespace MoreLockedDoors;
internal sealed class Implementation : MelonMod
{

    internal static SaveDataManager sdm = new SaveDataManager();
    public override void OnInitializeMelon()
	{
		MelonLogger.Msg("More Locked Doors is online.");
		ItemSpawnManager.InitializeCustomHandler();
	}

	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
	{
		LockManager.AddMysteryLakeLocks();
		LockManager.AddCoastalHighwayLocks();
		LockManager.AddCinderHillsCoalMineLocks();
		LockManager.AddDesolationPointLocks();
		LockManager.AddPleasantValleyLocks();
		LockManager.AddMountainTownLocks();
		LockManager.AddBlackrockLocks();
		LockManager.AddAshCanyonLocks();
		LockManager.AddBrokenRailroadLocks();
	}


}
