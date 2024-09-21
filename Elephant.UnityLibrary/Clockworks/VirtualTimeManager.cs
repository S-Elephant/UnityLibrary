#nullable enable

using System;
using UnityEngine;

namespace Elephant.UnityLibrary.Clockworks
{
	/// <summary>
	/// <p>Tracks and manages virtual time.</p>
	/// <p>Call <see cref="UpdateTime"/> to progress time and adjust the speed using <see cref="TimeScale"/>.</p>
	/// </summary>
	[Serializable]
	public class VirtualTimeManager
	{
		/// <summary>
		/// <p>Determines how fast virtual time progresses.</p>
		/// <p>A value of 60 means that 1 real second equals 1 virtual minute.</p>
		/// </summary>
		public float TimeScale;

		/// <inheritdoc cref="Second"/>
		[SerializeField] [Range(0f, 60f)] private float _second;

		/// <summary>
		/// Current second. Starts at 0.
		/// </summary>
		public float Second
		{
			get => _second;
			private set => _second = value;
		}

		/// <inheritdoc cref="Minute"/>
		[SerializeField] [Range(0, 59)] private int _minute;

		/// <summary>
		/// Current minute. Starts at 0.
		/// </summary>
		public int Minute
		{
			get => _minute;
			private set => _minute = value;
		}

		/// <inheritdoc cref="Hour"/>
		[SerializeField] [Range(0, 23)] private int _hour;

		/// <summary>
		/// Current hour. Starts at 0.
		/// </summary>
		public int Hour
		{
			get => _hour;
			private set => _hour = value;
		}

		/// <inheritdoc cref="Day"/>
		[SerializeField] [Min(0)] private int _day;

		/// <summary>
		/// Current day. Starts at 0.
		/// </summary>
		public int Day
		{
			get => _day;
			private set => _day = value;
		}

		/// <inheritdoc cref="Month"/>
		[SerializeField] [Range(1, 12)] private int _month;

		/// <summary>
		/// Current month. Starts at 1.
		/// </summary>
		public int Month
		{
			get => _month;
			private set => _month = value;
		}

		/// <inheritdoc cref="Year"/>
		[SerializeField] private int _year;

		/// <summary>
		/// Current year.
		/// </summary>
		public int Year
		{
			get => _year;
			private set => _year = value;
		}

		/// <inheritdoc cref="DaysInMonth"/>>
		private int[] _daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

		/// <summary>
		/// Defines the number of days in each month (ignoring leap years for simplicity).
		/// </summary>
		protected virtual int[] DaysInMonth => _daysInMonth;

		/// <summary>
		/// Maximum intensity of the light at noon.
		/// </summary>
		[Min(0)] public float IntensityMax = 1.0f;

		/// <summary>
		/// Minimum light intensity of the light at midnight.
		/// </summary>
		[Min(0)] public float IntensityMin = 0f;

		/// <summary>
		/// Is invoked if the season is changed.
		/// Is NOT invoked when the constructor is called.
		/// Is NOT invoked when manually adding time like for example <see cref="AddSeconds"/>.
		/// </summary>
		public Action<SeasonType, SeasonType>? OnSeasonChanged = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="timeScale">How fast virtual time progresses.</param>
		/// <param name="year">Initial year.</param>
		/// <param name="month">Initial month.</param>
		/// <param name="day">Initial day of the month.</param>
		/// <param name="hour">Initial hour of the day.</param>
		/// <param name="minute">Initial minute.</param>
		/// <param name="second">Initial second.</param>
		public VirtualTimeManager(float timeScale = 30f, int year = 2024, int month = 1, int day = 0, int hour = 0, int minute = 0, float second = 0f)
		{
			TimeScale = timeScale;
			Year = year;
			Month = month;
			Day = day;
			Hour = hour;
			Minute = minute;
			Second = second;
		}

		/// <summary>
		/// Sets the time to a specific time.
		/// </summary>
		/// <param name="year">Initial year.</param>
		/// <param name="month">Initial month.</param>
		/// <param name="day">Initial day of the month.</param>
		/// <param name="hour">Initial hour of the day.</param>
		/// <param name="minute">Initial minute.</param>
		/// <param name="second">Initial second.</param>
		/// <remarks>Going back into the past is allowed.</remarks>
		public virtual void SetTime(int year, int month, int day, int hour, int minute, float second = 0f)
		{
			SeasonType oldSeason = CurrentSeason();

			Year = year;
			Month = month;
			Day = day;
			Hour = hour;
			Minute = minute;
			Second = second;

			CheckAndProcessSeasonChanged(oldSeason);
		}

		/// <summary>
		/// Checks if the season changed and if so, attempts to invoke <see cref="OnSeasonChanged"/>.
		/// </summary>
		/// <param name="oldSeason">The other season used to detect if the season changed.</param>
		private void CheckAndProcessSeasonChanged(SeasonType oldSeason)
		{
			SeasonType currentSeason = CurrentSeason();
			if (oldSeason != currentSeason)
				OnSeasonChanged?.Invoke(oldSeason, currentSeason);
		}

		/// <summary>
		/// Call this method to advance time normally.
		/// </summary>
		public virtual void UpdateTime(float deltaTime)
		{
			SeasonType oldSeason = CurrentSeason();

			AddSeconds(deltaTime * TimeScale);

			CheckAndProcessSeasonChanged(oldSeason);
		}

		/// <summary>
		/// Returns the light intensity for the current <see cref="Hour"/>.
		/// </summary>
		/// <example>If you have a 2D sun (a Monobehaviour with a Light2D (light type: global)),
		/// then you could add this in the update loop:
		/// Light2D.intensity = VirtualTimeManager.CurrentLightIntensity();
		/// Also works for various other applications.</example>
		public virtual float CurrentLightIntensity()
		{
			return LightIntensity(ConvertToTotalHours(Hour, Minute, Second));
		}

		/// <summary>
		/// Converts hours, minutes, and seconds into a total number of hours as a float.
		/// </summary>
		/// <param name="hours">Number of hours.</param>
		/// <param name="minutes">Number of minutes.</param>
		/// <param name="seconds">Number of seconds as a float.</param>
		/// <returns>The total hours represented as a float. I.e. 1 hour and 30 minutes will return: 1.5f.</returns>
		public static float ConvertToTotalHours(int hours, int minutes, float seconds)
		{
			return hours + (minutes / 60.0f) + (seconds / 3600.0f);
		}

		/// <summary>
		/// Returns the light intensity for the <paramref name="hour"/>.
		/// </summary>
		public virtual float LightIntensity(float hour)
		{
			if (hour >= 6 && hour <= 18) // Day time from 6 AM to 6 PM
			{
				// Scale intensity up to maxIntensity at noon (hour 12) then down to evening
				float middayFactor = 1 - Mathf.Abs(hour - 12) / 6; // Peaks at noon.
				return Mathf.Lerp(IntensityMin, IntensityMax, middayFactor);
			}

			// Adjust for evening hours.
			if (hour > 18)
				hour -= 24;

			// Fade out to minIntensity towards midnight and from midnight towards morning.
			float nightFactor = 1 - Mathf.Abs(hour + 6) / 6; // Lowest at midnight.
			return Mathf.Lerp(IntensityMin, IntensityMax, nightFactor);
		}

		/// <summary>
		/// Returns the <see cref="SeasonType"/> based on the current date.
		/// </summary>
		public virtual SeasonType CurrentSeason()
		{
			return Season(Month, Day);
		}

		/// <summary>
		/// Returns the <see cref="SeasonType"/> based on the <paramref name="month"/> and <paramref name="day"/>.
		/// </summary>
		public static SeasonType Season(int month, int day)
		{
			// Check for winter: December 21st to March 20th.
			if ((month == 12 && day >= 21) || (month == 1) || (month == 2) || (month == 3 && day < 21))
				return SeasonType.Winter;

			// Check for spring: March 21st to June 20th.
			if ((month == 3 && day >= 21) || (month == 4) || (month == 5) || (month == 6 && day < 21))
				return SeasonType.Spring;

			// Check for summer: June 21st to September 22nd.
			if ((month == 6 && day >= 21) || (month == 7) || (month == 8) || (month == 9 && day < 22))
				return SeasonType.Summer;

			// If none of the above, it must be autumn: September 22nd to December 20th.
			return SeasonType.Fall;
		}

		#region Add

		/// <summary>
		/// Add <paramref name="seconds"/>.
		/// </summary>
		public virtual void AddSeconds(float seconds)
		{
			if (seconds < 0f)
			{
				SubtractSeconds(-seconds);
				return;
			}

			Second += seconds;
			int minutesToAdd = (int)Second / 60;
			Second %= 60;

			AddMinutes(minutesToAdd);

			if (Minute >= 60)
			{
				AddMinutes(Minute / 60);
				Minute %= 60;
			}
		}

		/// <summary>
		/// Add <paramref name="minutes"/>.
		/// </summary>
		public virtual void AddMinutes(int minutes)
		{
			if (minutes < 0f)
			{
				SubtractMinutes(-minutes);
				return;
			}

			Minute += minutes;
			int hoursToAdd = Minute / 60;
			Minute %= 60;

			AddHours(hoursToAdd);
		}

		/// <summary>
		/// Add <paramref name="hours"/>.
		/// </summary>
		public virtual void AddHours(int hours)
		{
			if (hours < 0f)
			{
				SubtractHours(-hours);
				return;
			}

			Hour += hours;
			int daysToAdd = Hour / 24;
			Hour %= 24;

			AddDays(daysToAdd);
		}

		/// <summary>
		/// Add <paramref name="days"/>.
		/// </summary>
		public virtual void AddDays(int days)
		{
			if (days < 0f)
			{
				SubtractDays(-days);
				return;
			}

			Day += days;
			while (Day > DaysInMonth[Month - 1])
			{
				Day -= DaysInMonth[Month - 1];
				AddMonths(1);
			}
		}

		/// <summary>
		/// Add <paramref name="months"/>.
		/// </summary>
		public virtual void AddMonths(int months)
		{
			if (months < 0f)
			{
				SubtractMonths(-months);
				return;
			}

			Month += months;
			while (Month > 12)
			{
				Month -= 12;
				AddYears(1);
			}
		}

		/// <summary>
		/// Add <paramref name="years"/>.
		/// </summary>
		public virtual void AddYears(int years)
		{
			Year += years;
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Add <paramref name="seconds"/>.
		/// </summary>
		public virtual void SubtractSeconds(float seconds)
		{
			if (seconds < 0f)
			{
				AddSeconds(-seconds);
				return;
			}

			Second -= seconds;
			while (Second < 0)
			{
				SubtractMinutes(1);
				Second += 60;
			}

			int minutesToSubtract = (int)Second / 60;
			Second %= 60;

			SubtractMinutes(minutesToSubtract);

			if (Minute < 0)
			{
				SubtractMinutes(1);
				Minute += 60;
			}
		}

		/// <summary>
		/// Add <paramref name="minutes"/>.
		/// </summary>
		public virtual void SubtractMinutes(int minutes)
		{
			if (minutes < 0f)
			{
				AddMinutes(-minutes);
				return;
			}

			Minute -= minutes;
			while (Minute < 0)
			{
				SubtractHours(1);
				Minute += 60;
			}
		}

		/// <summary>
		/// Add <paramref name="hours"/>.
		/// </summary>
		public virtual void SubtractHours(int hours)
		{
			if (hours < 0f)
			{
				AddHours(-hours);
				return;
			}

			Hour -= hours;
			while (Hour < 0)
			{
				SubtractDays(1);
				Hour += 24;
			}
		}

		/// <summary>
		/// Subtract <paramref name="days"/>.
		/// </summary>
		public virtual void SubtractDays(int days)
		{
			if (days < 0f)
			{
				AddDays(-days);
				return;
			}

			Day -= days;
			while (Day <= 0)
			{
				AddMonths(-1);
				Day += DaysInMonth[Month - 1];
			}
		}

		/// <summary>
		/// Subtract <paramref name="months"/>.
		/// </summary>
		public virtual void SubtractMonths(int months)
		{
			if (months < 0f)
			{
				AddMonths(-months);
				return;
			}

			Month -= months;
			while (Month <= 0)
			{
				Month += 12;
				AddYears(-1);
			}
		}

		/// <summary>
		/// Subtract <paramref name="years"/>.
		/// </summary>
		public virtual void SubtractYears(int years)
		{
			Year -= years;
		}

		#endregion

		#region ToStrings

		/// <summary>
		/// Returns the current time in 24-hour format (HH:mm).
		/// </summary>
		/// <returns>A string representing the current time in 24-hour format.</returns>
		public string ToTime24HourFormat()
		{
			return $"{Hour:00}:{Minute:00}";
		}

		/// <summary>
		/// Returns the current time in 12-hour format with AM/PM (hh:mm tt).
		/// </summary>
		/// <returns>A string representing the current time in 12-hour format.</returns>
		public string ToTime12HourFormat()
		{
			string amPm = Hour >= 12 ? "PM" : "AM";
			int hour12 = Hour % 12;

			// Convert hour '0' to '12' for 12-hour format purposes.
			if (hour12 == 0)
				hour12 = 12;

			return $"{hour12:00}:{Minute:00} {amPm}";
		}

		/// <summary>
		/// Returns the current date in European format (DD/MM/YYYY).
		/// </summary>
		/// <returns>A string representing the current date in European format.</returns>
		public string ToDateEuropeanFormat()
		{
			return $"{Day:00}-{Month:00}-{Year}";
		}

		/// <summary>
		/// Returns the current date in US format (MM/DD/YYYY).
		/// </summary>
		/// <returns>A string representing the current date in US format.</returns>
		public string ToDateUsFormat()
		{
			return $"{Month:00}/{Day:00}/{Year}";
		}

		#endregion
	}
}