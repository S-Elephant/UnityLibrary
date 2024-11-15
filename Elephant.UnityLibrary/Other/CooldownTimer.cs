using System;
using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Manages a cooldown timer that can be updated, reset, and checked for readiness.
	/// </summary>
	[Serializable]
	public class CooldownTimer
	{
		/// <summary>
		/// Duration of the cooldown in seconds.
		/// </summary>
		[SerializeField] public float CooldownDuration;

		/// <summary>
		/// Time remaining in the cooldown in seconds.
		/// </summary>
		[SerializeField] public float TimeRemaining;

		/// <summary>
		/// Initializes the cooldown timer with a duration.
		/// </summary>
		/// <param name="cooldownDurationInSec">Cooldown duration in seconds.</param>
		/// <param name="timeRemaining">Optional initial <see cref="TimeRemaining"/>. Defaults to 0f.</param>
		public CooldownTimer(float cooldownDurationInSec, float timeRemaining = 0f)
		{
			CooldownDuration = cooldownDurationInSec;
			TimeRemaining = timeRemaining;
		}

		/// <summary>
		/// Updates the cooldown timer. Should be called every frame.
		/// </summary>
		/// <param name="deltaTime">Time elapsed since the last frame.</param>
		/// <returns>Value of <see cref="IsReady"/>.</returns>
		public bool Update(float deltaTime)
		{
			if (TimeRemaining > 0f)
				TimeRemaining -= deltaTime;

			return IsReady();
		}

		/// <summary>
		/// Checks if the cooldown is complete.
		/// </summary>
		/// <returns>true if the cooldown is complete, false otherwise.</returns>
		public bool IsReady()
		{
			return TimeRemaining <= float.Epsilon;
		}

		/// <summary>
		/// Resets the cooldown timer.
		/// </summary>
		public void Reset()
		{
			TimeRemaining = CooldownDuration;
		}

		/// <summary>
		/// Resets the cooldown timer with a new duration.
		/// </summary>
		/// <param name="cooldownDurationInSec">New duration (in seconds) for the cooldown.</param>
		public void Reset(float cooldownDurationInSec)
		{
			CooldownDuration = cooldownDurationInSec;
			TimeRemaining = CooldownDuration;
		}
	}
}
