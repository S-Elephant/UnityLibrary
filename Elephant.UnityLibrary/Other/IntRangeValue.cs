using System;
using UnityEngine;

// Note to developers: Don't use generics here because Unity and generic serialization is.. You know.. Pretty much non-existent.
namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Represents an integer value within a specified range, with methods to handle clamping and percentage calculations.
	/// </summary>
	[Serializable]
	public class IntRangeValue
	{
		/// <inheritdoc cref="Min"/>
		[SerializeField] private int _min;

		/// <inheritdoc cref="Max"/>
		[SerializeField] private int _max;

		/// <inheritdoc cref="Value"/>
		[SerializeField] private int _value;

		/// <summary>
		/// Minimum value.
		/// </summary>
		public int Min
		{
			get => _min;
			set
			{
				_min = value;

				if (_min > _max)
					_max = _min;

				ClampValue();
			}
		}

		/// <summary>
		/// Maximum value.
		/// </summary>
		public int Max
		{
			get => _max;
			set
			{
				_max = value;

				if (_max < _min)
					_min = _max;

				if (_value > _max)
					_value = _max;

				ClampValue();
			}
		}

		/// <summary>
		/// Current value.
		/// </summary>
		public int Value
		{
			get => _value;
			set => _value = Mathf.Clamp(value, _min, _max);
		}

		/// <summary>
		/// Current value as a percentage between 0f and 1f.
		/// </summary>
		public float Percentage
		{
			get
			{
				// Calculate the percentage of the current value within the range [Min, Max].
				// If Max is greater than Min, compute the percentage; otherwise, return 0f.
				return (Max > Min) ? (float)(Value - Min) / (Max - Min) : 0f;
			}

			set
			{
				// Calculate the corresponding value based on the given percentage.
				// Scale the percentage to the range [Min, Max] and clamp the result.
				Value = Mathf.Clamp(Mathf.RoundToInt(Min + value * (Max - Min)), Min, Max);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public IntRangeValue()
		{
			_min = 0;
			_max = 0;
			Value = 0;
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public IntRangeValue(int value, int min, int max)
		{
			_min = min;
			_max = max;
			Value = value;
		}

		/// <summary>
		/// Clamps the current value between the minimum and maximum values.
		/// </summary>
		private void ClampValue()
		{
			_value = Mathf.Clamp(_value, _min, _max);
		}

		/// <summary>
		/// Returns true if <see cref="Value"/> is 0, <see cref="Min"/> is 0 and <see cref="Max"/> is 0.
		/// </summary>
		public bool IsEmpty()
		{
			return _value == 0 && _min == 0 && _max == 0;
		}

		#region Operators

		/// <summary>
		/// Adds an integer value to an <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="IntRangeValue"/> to modify.</param>
		/// <param name="b">Integer value to add.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator +(IntRangeValue a, int b)
		{
			return new IntRangeValue(a.Value + b, a.Min, a.Max);
		}

		/// <summary>
		/// Subtracts an integer value from an <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="IntRangeValue"/> to modify.</param>
		/// <param name="b">Integer value to subtract.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator -(IntRangeValue a, int b)
		{
			return new IntRangeValue(a.Value - b, a.Min, a.Max);
		}

		/// <summary>
		/// Multiplies an <see cref="IntRangeValue"/> by an integer value and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="IntRangeValue"/> to modify.</param>
		/// <param name="b">Integer value to multiply by.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator *(IntRangeValue a, int b)
		{
			return new IntRangeValue(a.Value * b, a.Min, a.Max);
		}

		/// <summary>
		/// Divides an <see cref="IntRangeValue"/> by an integer value and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="IntRangeValue"/> to modify.</param>
		/// <param name="b">Integer value to divide by.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator /(IntRangeValue a, int b)
		{
			return new IntRangeValue(a.Value / b, a.Min, a.Max);
		}

		/// <summary>
		/// Checks if the <see cref="IntRangeValue"/> instance is equal to an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is equal to the integer value; otherwise, false.</returns>
		public static bool operator ==(IntRangeValue a, int b)
		{
			return a._value == b;
		}

		/// <summary>
		/// Checks if the <see cref="IntRangeValue"/> instance is not equal to an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is not equal to the integer value; otherwise, false.</returns>
		public static bool operator !=(IntRangeValue a, int b)
		{
			return a._value != b;
		}

		/// <summary>
		/// Compares if the <see cref="IntRangeValue"/> instance's value is less than an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is less than the integer value; otherwise, false.</returns>
		public static bool operator <(IntRangeValue a, int b)
		{
			return a._value < b;
		}

		/// <summary>
		/// Compares if the <see cref="IntRangeValue"/> instance's value is greater than an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is greater than the integer value; otherwise, false.</returns>
		public static bool operator >(IntRangeValue a, int b)
		{
			return a._value > b;
		}

		/// <summary>
		/// Compares if the <see cref="IntRangeValue"/> instance's value is less than or equal to an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is less than or equal to the integer value; otherwise, false.</returns>
		public static bool operator <=(IntRangeValue a, int b)
		{
			return a._value <= b;
		}

		/// <summary>
		/// Compares if the <see cref="IntRangeValue"/> instance's value is greater than or equal to an integer value.
		/// </summary>
		/// <param name="a">The <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">The integer value to compare against.</param>
		/// <returns>True if the <see cref="IntRangeValue"/> instance's value is greater than or equal to the integer value; otherwise, false.</returns>
		public static bool operator >=(IntRangeValue a, int b)
		{
			return a._value >= b;
		}

		/// <summary>
		/// Adds an <see cref="IntRangeValue"/> to another <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance to add.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator +(IntRangeValue a, IntRangeValue b)
		{
			return new IntRangeValue(a.Value + b.Value, a.Min + b.Min, a.Max + b.Max);
		}

		/// <summary>
		/// Subtracts an <see cref="IntRangeValue"/> from another <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance to subtract.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator -(IntRangeValue a, IntRangeValue b)
		{
			return new IntRangeValue(a.Value - b.Value, a.Min - b.Min, a.Max - b.Max);
		}

		/// <summary>
		/// Multiplies an <see cref="IntRangeValue"/> by another <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance to multiply by.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator *(IntRangeValue a, IntRangeValue b)
		{
			return new IntRangeValue(a.Value * b.Value, a.Min * b.Min, a.Max * b.Max);
		}

		/// <summary>
		/// Divides an <see cref="IntRangeValue"/> by another <see cref="IntRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance to divide by.</param>
		/// <returns>A new <see cref="IntRangeValue"/> instance with the updated value.</returns>
		public static IntRangeValue operator /(IntRangeValue a, IntRangeValue b)
		{
			return new IntRangeValue(a.Value / b.Value, a.Min / b.Min, a.Max / b.Max);
		}

		/// <summary>
		/// Checks if two <see cref="IntRangeValue"/> instances are equal.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if both instances have the same values, min, and max; otherwise, false.</returns>
		public static bool operator ==(IntRangeValue a, IntRangeValue b)
		{
			return a._value == b._value && a._min == b._min && a._max == b._max;
		}

		/// <summary>
		/// Checks if two <see cref="IntRangeValue"/> instances are not equal.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if the instances have different values, min, or max; otherwise, false.</returns>
		public static bool operator !=(IntRangeValue a, IntRangeValue b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Compares if the value of the first <see cref="IntRangeValue"/> instance is less than the second instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is less than the second instance; otherwise, false.</returns>
		public static bool operator <(IntRangeValue a, IntRangeValue b)
		{
			return a._value < b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="IntRangeValue"/> instance is greater than the second instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is greater than the second instance; otherwise, false.</returns>
		public static bool operator >(IntRangeValue a, IntRangeValue b)
		{
			return a._value > b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="IntRangeValue"/> instance is less than or equal to the second instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is less than or equal to the second instance; otherwise, false.</returns>
		public static bool operator <=(IntRangeValue a, IntRangeValue b)
		{
			return a._value <= b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="IntRangeValue"/> instance is greater than or equal to the second instance.
		/// </summary>
		/// <param name="a">First <see cref="IntRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="IntRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is greater than or equal to the second instance; otherwise, false.</returns>
		public static bool operator >=(IntRangeValue a, IntRangeValue b)
		{
			return a._value >= b._value;
		}

		#endregion

		/// <inheritdoc/>
		public override string ToString()
		{
			return Value.ToString();
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (obj is IntRangeValue other)
				return _value == other._value && _min == other._min && _max == other._max;

			return false;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = _value.GetHashCode();
				hashCode = (hashCode * 397) ^ _min.GetHashCode();
				hashCode = (hashCode * 397) ^ _max.GetHashCode();
				return hashCode;
			}
		}
	}
}
