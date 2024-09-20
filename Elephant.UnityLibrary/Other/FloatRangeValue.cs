using System;
using UnityEngine;

// Note to developers: Don't use generics here because Unity and generic serialization is.. You know.. Pretty much non-existent.
namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Represents a value within a specified range, with methods to handle clamping and percentage calculations.
	/// </summary>
	[Serializable]
	public class FloatRangeValue
	{
		/// <summary>
		/// Floating point tolerance for all <see cref="FloatRangeValue"/>s. Defaults to <see cref="float.Epsilon"/>.
		/// </summary>
		public static float Tolerance = float.Epsilon;

		/// <inheritdoc cref="Min"/>
		[SerializeField] private float _min;

		/// <inheritdoc cref="Max"/>
		[SerializeField] private float _max;

		/// <inheritdoc cref="Value"/>
		[SerializeField] private float _value;

		/// <summary>
		/// Minimum value.
		/// </summary>
		public float Min
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
		public float Max
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
		public float Value
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
				return (Max > Min) ? (Value - Min) / (Max - Min) : 0f;
			}

			set
			{
				// Calculate the corresponding value based on the given percentage.
				// Scale the percentage to the range [Min, Max] and clamp the result.
				Value = Mathf.Clamp(Min + value * (Max - Min), Min, Max);
			}
		}

		/// <summary>
		/// Returns true if <see cref="Value"/> equals <see cref="Min"/>.
		/// <see cref="Tolerance"/> is taken into account.
		/// </summary>
		public bool IsMinValue => Value <= Min + Tolerance;

		/// <summary>
		/// Returns true if <see cref="Value"/> equals <see cref="Max"/>.
		/// <see cref="Tolerance"/> is taken into account.
		/// </summary>
		public bool IsMaxValue => Value >= Max - Tolerance;

		/// <summary>
		/// Constructor.
		/// </summary>
		public FloatRangeValue()
		{
			_min = 0;
			_max = 0;
			Value = 0;
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public FloatRangeValue(float value, float min, float max)
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
			return Mathf.Abs(_value) < Mathf.Epsilon && Mathf.Abs(Min) < Mathf.Epsilon && Mathf.Abs(Max) < Mathf.Epsilon;
		}

		/// <summary>
		/// Assigns <paramref name="newMinAndValue"/> to <see cref="Min"/> and then to <see cref="Value"/>.
		/// </summary>
		public void SetMinAndMatchValue(float newMinAndValue)
		{
			Min = newMinAndValue;
			Value = Min;
		}

		/// <summary>
		/// Assigns <paramref name="newMaxAndValue"/> to <see cref="Max"/> and then to <see cref="Value"/>.
		/// </summary>
		public void SetMaxAndMatchValue(float newMaxAndValue)
		{
			Max = newMaxAndValue;
			Value = Max;
		}

		#region Operators

		/// <summary>
		/// Adds a float value to a <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="FloatRangeValue"/> to modify.</param>
		/// <param name="b">Float value to add.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator +(FloatRangeValue a, float b)
		{
			return new FloatRangeValue(a.Value + b, a.Min, a.Max);
		}

		/// <summary>
		/// Subtracts a float value from a <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="FloatRangeValue"/> to modify.</param>
		/// <param name="b">Float value to subtract.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator -(FloatRangeValue a, float b)
		{
			return new FloatRangeValue(a.Value - b, a.Min, a.Max);
		}

		/// <summary>
		/// Multiplies a <see cref="FloatRangeValue"/> by a float value and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="FloatRangeValue"/> to modify.</param>
		/// <param name="b">Float value to multiply by.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator *(FloatRangeValue a, float b)
		{
			return new FloatRangeValue(a.Value * b, a.Min, a.Max);
		}

		/// <summary>
		/// Divides a <see cref="FloatRangeValue"/> by a float value and returns a new instance.
		/// </summary>
		/// <param name="a"><see cref="FloatRangeValue"/> to modify.</param>
		/// <param name="b">Float value to divide by.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator /(FloatRangeValue a, float b)
		{
			return new FloatRangeValue(a.Value / b, a.Min, a.Max);
		}

		/// <summary>
		/// Checks if the <see cref="FloatRangeValue"/> instance is equal to a float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is equal to the float value; otherwise, false.</returns>
		public static bool operator ==(FloatRangeValue a, float b)
		{
			return a._value == b;
		}

		/// <summary>
		/// Checks if the <see cref="FloatRangeValue"/> instance is not equal to a float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is not equal to the float value; otherwise, false.</returns>
		public static bool operator !=(FloatRangeValue a, float b)
		{
			return a._value != b;
		}

		/// <summary>
		/// Compares if the <see cref="FloatRangeValue"/> instance's value is less than the float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is less than the float value; otherwise, false.</returns>
		public static bool operator <(FloatRangeValue a, float b)
		{
			return a._value < b;
		}

		/// <summary>
		/// Compares if the <see cref="FloatRangeValue"/> instance's value is greater than the float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is greater than the float value; otherwise, false.</returns>
		public static bool operator >(FloatRangeValue a, float b)
		{
			return a._value > b;
		}

		/// <summary>
		/// Compares if the <see cref="FloatRangeValue"/> instance's value is less than or equal to the float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is less than or equal to the float value; otherwise, false.</returns>
		public static bool operator <=(FloatRangeValue a, float b)
		{
			return a._value <= b;
		}

		/// <summary>
		/// Compares if the <see cref="FloatRangeValue"/> instance's value is greater than or equal to the float value.
		/// </summary>
		/// <param name="a">The <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">The float value to compare against.</param>
		/// <returns>True if the <see cref="FloatRangeValue"/> instance's value is greater than or equal to the float value; otherwise, false.</returns>
		public static bool operator >=(FloatRangeValue a, float b)
		{
			return a._value >= b;
		}

		/// <summary>
		/// Adds a <see cref="FloatRangeValue"/> to another <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance to add.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator +(FloatRangeValue a, FloatRangeValue b)
		{
			return new FloatRangeValue(a.Value + b.Value, a.Min + b.Min, a.Max + b.Max);
		}

		/// <summary>
		/// Subtracts a <see cref="FloatRangeValue"/> from another <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance to subtract.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator -(FloatRangeValue a, FloatRangeValue b)
		{
			return new FloatRangeValue(a.Value - b.Value, a.Min - b.Min, a.Max - b.Max);
		}

		/// <summary>
		/// Multiplies a <see cref="FloatRangeValue"/> by another <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance to multiply by.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator *(FloatRangeValue a, FloatRangeValue b)
		{
			return new FloatRangeValue(a.Value * b.Value, a.Min * b.Min, a.Max * b.Max);
		}

		/// <summary>
		/// Divides a <see cref="FloatRangeValue"/> by another <see cref="FloatRangeValue"/> and returns a new instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance to divide by.</param>
		/// <returns>A new <see cref="FloatRangeValue"/> instance with the updated value.</returns>
		public static FloatRangeValue operator /(FloatRangeValue a, FloatRangeValue b)
		{
			return new FloatRangeValue(a.Value / b.Value, a.Min / b.Min, a.Max / b.Max);
		}

		/// <summary>
		/// Checks if two <see cref="FloatRangeValue"/> instances are equal.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if both instances have the same values, min, and max; otherwise, false.</returns>
		public static bool operator ==(FloatRangeValue a, FloatRangeValue b)
		{
			return a._value == b._value && a._min == b._min && a._max == b._max;
		}

		/// <summary>
		/// Checks if two <see cref="FloatRangeValue"/> instances are not equal.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if the instances have different values, min, or max; otherwise, false.</returns>
		public static bool operator !=(FloatRangeValue a, FloatRangeValue b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Compares if the value of the first <see cref="FloatRangeValue"/> instance is less than the second instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is less than the second instance; otherwise, false.</returns>
		public static bool operator <(FloatRangeValue a, FloatRangeValue b)
		{
			return a._value < b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="FloatRangeValue"/> instance is greater than the second instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is greater than the second instance; otherwise, false.</returns>
		public static bool operator >(FloatRangeValue a, FloatRangeValue b)
		{
			return a._value > b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="FloatRangeValue"/> instance is less than or equal to the second instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is less than or equal to the second instance; otherwise, false.</returns>
		public static bool operator <=(FloatRangeValue a, FloatRangeValue b)
		{
			return a._value <= b._value;
		}

		/// <summary>
		/// Compares if the value of the first <see cref="FloatRangeValue"/> instance is greater than or equal to the second instance.
		/// </summary>
		/// <param name="a">First <see cref="FloatRangeValue"/> instance.</param>
		/// <param name="b">Second <see cref="FloatRangeValue"/> instance.</param>
		/// <returns>True if the value of the first instance is greater than or equal to the second instance; otherwise, false.</returns>
		public static bool operator >=(FloatRangeValue a, FloatRangeValue b)
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
			if (obj is FloatRangeValue other)
			{
				return Math.Abs(_value - other._value) < Tolerance &&
					   Math.Abs(_min - other._min) < Tolerance &&
					   Math.Abs(_max - other._max) < Tolerance;
			}

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
