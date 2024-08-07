using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using CruzersLocoTweaks.Shared;
using CruzersLocoTweaks.Parts;


namespace CruzersLocoTweaks.PartsTweaker
{
	internal class PartsSelectorPointAtNothing : AStateBehaviour
	{
		private const float SIGNAL_RANGE = 100f;

		private Transform signalOrigin;
		private int trainCarMask;

		private CarHighlighter highlighter;

		public PartsSelectorPointAtNothing()
			: base(new CommsRadioState(
				titleText: "Loco Parts",
				contentText: "Aim at a locomotive you wish to customize.",
				actionText: "Cancel",
				buttonBehaviour: ButtonBehaviourType.Override))
		{
			highlighter = new CarHighlighter();
		}

		public override void OnEnter(CommsRadioUtility utility, AStateBehaviour? previous)
		{
			base.OnEnter(utility, previous);
			// Steal some components from other radio modes
			refreshSignalOriginAndTrainCarMask();
		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			if (action != InputAction.Activate)
			{
				throw new ArgumentException();
			}

			utility.PlaySound(VanillaSoundCommsRadio.Cancel);
			return new PartsSelectorStart();
		}

		private void refreshSignalOriginAndTrainCarMask()
		{
			trainCarMask = highlighter.RefreshTrainCarMask();
			signalOrigin = highlighter.RefreshSignalOrigin();
		}

		// Detecting what we're looking at
		public override AStateBehaviour OnUpdate(CommsRadioUtility utility)
		{
			while (signalOrigin is null)
			{
				Main.DebugLog("signalOrigin is null for some reason");
				refreshSignalOriginAndTrainCarMask();
			}

			RaycastHit hit;

			// If we're not pointing at anything
			if (!Physics.Raycast(signalOrigin.position, signalOrigin.forward, out hit, SIGNAL_RANGE, trainCarMask))
			{
				return this;
			}

			// Try to get the car we're pointing at
			TrainCar selectedCar = TrainCar.Resolve(hit.transform.root);

			// If we aren't pointing at a car
			if (selectedCar is null)
			{
				return this;
			}

			// If we're pointing at an available car
			if (Enum.IsDefined(typeof(AvailableLocos), (AvailableLocos)selectedCar.carType))
			{
				utility.PlaySound(VanillaSoundCommsRadio.HoverOver);
				return new PartsSelectorPointAtLoco(selectedCar);
			}
			else
			{
				return this;
			}
		}
	}
}
