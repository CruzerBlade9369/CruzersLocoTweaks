using System;
using System.Reflection;

using HarmonyLib;
using UnityModManagerNet;

using DV;

using UnityEngine;

using CommsRadioAPI;
using CruzersLocoTweaks.PartsTweaker;

namespace CruzersLocoTweaks
{
	[EnableReloading]
	public static class Main
	{
		public static bool enabled;
		public static UnityModManager.ModEntry? mod;
		public static Settings settings = new Settings();
		public static CommsRadioMode CommsRadioMode { get; private set; }

		private static readonly Color LASER_COLOR = new Color(0.5f, 0f, 1f);

		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Harmony? harmony = null;

			try
			{
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());
				DebugLog("Attempting patch.");

				mod = modEntry;
				modEntry.OnGUI = OnGui;
				modEntry.OnSaveGUI = OnSaveGui;

				ControllerAPI.Ready += StartCommsRadio;
			}
			catch (Exception ex)
			{
				modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
				harmony?.UnpatchAll(modEntry.Info.Id);
				return false;
			}

			return true;
		}

		static void OnGui(UnityModManager.ModEntry modEntry)
		{
			settings.Draw(modEntry);
		}

		static void OnSaveGui(UnityModManager.ModEntry modEntry)
		{
			settings.Save(modEntry);
		}

		public static void DebugLog(string message)
		{
			if (settings.isLoggingEnabled)
				mod?.Logger.Log(message);
		}

		public static void StartCommsRadio()
		{
			CommsRadioMode = CommsRadioMode.Create(new PartsSelectorStart(), LASER_COLOR, (mode) => mode is CommsRadioCrewVehicle);
		}
	}
}
