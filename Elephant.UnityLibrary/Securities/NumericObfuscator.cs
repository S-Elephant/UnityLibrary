using Elephant.UnityLibrary.Securities.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.Securities
{
	/// <summary>
	/// Simple obfuscator that adds a random offset to obscure them.
	/// </summary>
	public class NumericObfuscator : INumericObfuscator
	{
		/// <summary>
		/// Integer offset used to both obfuscate and deobfuscate.
		/// </summary>
		private readonly int _obfuscationOffset;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="obfuscationOffset">
		/// Preset integer offset. Don't use the value 0.
		/// Save this if you want to deobfuscate after an application restart.
		/// Don't make this value too low or too high to prevent overflows.
		/// </param>
		public NumericObfuscator(int obfuscationOffset)
		{
			if (obfuscationOffset == 0)
				Debug.LogWarning($"Using an {nameof(obfuscationOffset)} of 0 which means no obfuscation.");

			_obfuscationOffset = obfuscationOffset;
		}

		/// <inheritdoc/>
		public decimal Obfuscate(decimal realValue) => realValue + _obfuscationOffset;

		/// <inheritdoc/>
		public double Obfuscate(double realValue) => realValue + _obfuscationOffset;

		/// <inheritdoc/>
		public float Obfuscate(float realValue) => realValue + _obfuscationOffset;

		/// <inheritdoc/>
		public int Obfuscate(int realValue) => realValue + _obfuscationOffset;

		/// <inheritdoc/>
		public long Obfuscate(long realValue) => realValue + _obfuscationOffset;

		/// <inheritdoc/>
		public decimal DeObfuscate(decimal obfuscatedValue) => obfuscatedValue - _obfuscationOffset;

		/// <inheritdoc/>
		public double DeObfuscate(double obfuscatedValue) => obfuscatedValue - _obfuscationOffset;

		/// <inheritdoc/>
		public float DeObfuscate(float obfuscatedValue) => obfuscatedValue - _obfuscationOffset;

		/// <inheritdoc/>
		public int DeObfuscate(int obfuscatedValue) => obfuscatedValue - _obfuscationOffset;

		/// <inheritdoc/>
		public long DeObfuscate(long obfuscatedValue) => obfuscatedValue - _obfuscationOffset;
	}
}
