using CommsRadioAPI;
using DV;
using CruzersLocoTweaks.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruzersLocoTweaks.PartsTweaker
{
	internal class PartsSelectorStart : AStateBehaviour
	{
		public PartsSelectorStart()
			: base(new CommsRadioState(
				titleText: "Loco Parts",
				contentText: "Enable locomotive parts customizer?",
				buttonBehaviour: ButtonBehaviourType.Regular))
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			if (action != InputAction.Activate)
			{
				throw new ArgumentException();
			}

			utility.PlaySound(VanillaSoundCommsRadio.Confirm);
			return new PartsSelectorPointAtNothing();
		}
	}
}
