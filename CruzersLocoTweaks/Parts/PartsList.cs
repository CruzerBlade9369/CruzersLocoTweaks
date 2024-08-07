using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruzersLocoTweaks.Parts
{
	public enum AvailableLocos
	{
		LocoSteamHeavy = 20,
	}

	internal class PartsList
	{
		public static void ListLocoParts()
		{
			var locomotiveParts = new Dictionary<string, Dictionary<string, List<string>>>
			{
				{
					"LocoSteamHeavy", new Dictionary<string, List<string>>
					{
						{ "funnel", new List<string> { "default", "short" } },
						{ "pilot", new List<string> { "default", "guard iron only" } },
						{ "cab windows", new List<string> { "default", "square" } },
						{ "front railings", new List<string> { "enabled", "disabled" } },
						{ "firebox", new List<string> { "default", "wide", "belpaire", "wide belpaire" } },
						{ "smoke deflectors", new List<string> { "disabled", "wagner" } },
						{ "bunny ears", new List<string> { "disabled", "wagner" } },
					}
				}
			};
		}
	}
}
