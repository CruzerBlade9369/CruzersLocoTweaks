using System;

using CommsRadioAPI;

namespace CruzersLocoTweaks.PartsTweaker
{
	internal class PartsSelectorPointAtLoco : PartsSelectorPointAtLocoState
	{
		public PartsSelectorPointAtLoco(TrainCar selectedCar)
			: base(selectedCar)
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			if (action != InputAction.Activate)
			{
				throw new ArgumentException();
			}

			utility.PlaySound(VanillaSoundCommsRadio.Switch);
			return new PartsSelectorPointAtLoco(selectedCar);
		}
	}
}

