using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using CruzersLocoTweaks.Shared;

namespace CruzersLocoTweaks.PartsTweaker
{
	internal abstract class PartsSelectorPointAtLocoState : AStateBehaviour
	{
		private const float SIGNAL_RANGE = 200f;

		internal TrainCar selectedCar;
		private Transform signalOrigin;
		private CommsRadioCarDeleter carDeleter;
		private int trainCarMask;

		private CarHighlighter highlighter;

		public PartsSelectorPointAtLocoState(TrainCar selectedCar)
			: base(new CommsRadioState(
				titleText: "Loco Parts",
				contentText: $"Modifying [PART] of {selectedCar.ID}",
				actionText: "[PART STATUS]",
				buttonBehaviour: ButtonBehaviourType.Override))
		{
			this.selectedCar = selectedCar;

			if (this.selectedCar is null)
			{
				Main.DebugLog("selectedCar is null");
				throw new ArgumentException(nameof(selectedCar));
			}

			highlighter = new CarHighlighter();

			carDeleter = highlighter.RefreshCarDeleterComponent();
			signalOrigin = carDeleter.signalOrigin;
			highlighter.InitHighlighter(selectedCar, carDeleter);
		}

		public void Awake()
		{

		}

		public override AStateBehaviour OnUpdate(CommsRadioUtility utility)
		{
			RaycastHit hit;
			//if we're not pointing at anything
			if (!Physics.Raycast(signalOrigin.position, signalOrigin.forward, out hit, SIGNAL_RANGE, trainCarMask))
			{
				return new PartsSelectorPointAtNothing();
			}
			TrainCar target = TrainCar.Resolve(hit.transform.root);
			if (target is null || target != selectedCar)
			{
				//if we stopped pointing at selectedCar and are now pointing at either
				//nothing or another train car, then go back to PointingAtNothing so
				//we can figure out what we're pointing at
				return new PartsSelectorPointAtNothing();
			}
			return this;
		}

		public override void OnEnter(CommsRadioUtility utility, AStateBehaviour? previous)
		{
			base.OnEnter(utility, previous);
			trainCarMask = highlighter.RefreshTrainCarMask();
			highlighter.StartHighlighter(utility, previous, true);
		}

		public override void OnLeave(CommsRadioUtility utility, AStateBehaviour? next)
		{
			base.OnLeave(utility, next);
			highlighter.StopHighlighter(utility, next);
		}
	}
}
