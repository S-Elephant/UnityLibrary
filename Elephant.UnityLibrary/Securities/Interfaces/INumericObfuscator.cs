namespace Elephant.UnityLibrary.Securities.Interfaces
{
	/// <summary>
	/// Can obfuscate and deobfuscate some numeric types.
	/// </summary>
	/// <remarks>
	/// Doesn't use the INumnber interface because it may not
	/// work on all mobile platforms at this point in timetime.
	/// </remarks>
	public interface INumericObfuscator
	{
		/// <summary>
		/// Obfuscate a decimal value.
		/// </summary>
		/// <param name="realValue">Original decimal value to obfuscate.</param>
		/// <returns>Obfuscated decimal value.</returns>
		decimal Obfuscate(decimal realValue);

		/// <summary>
		/// Obfuscate an double value.
		/// </summary>
		/// <param name="realValue">Original double value to obfuscate.</param>
		/// <returns>Obfuscated double value.</returns>
		double Obfuscate(double realValue);

		/// <summary>
		/// Obfuscate a float value.
		/// </summary>
		/// <param name="realValue">Original float value to obfuscate.</param>
		/// <returns>Obfuscated float value.</returns>
		float Obfuscate(float realValue);

		/// <summary>
		/// Obfuscate an integer value.
		/// </summary>
		/// <param name="realValue">Original integer value to obfuscate.</param>
		/// <returns>Obfuscated integer value.</returns>
		int Obfuscate(int realValue);

		/// <summary>
		/// Obfuscate a long value.
		/// </summary>
		/// <param name="realValue">Original long value to obfuscate.</param>
		/// <returns>Obfuscated long value.</returns>
		long Obfuscate(long realValue);

		/// <summary>
		/// Deobfuscate an decimal value.
		/// </summary>
		/// <param name="obfuscatedValue">Obfuscated decimal value to deobfuscate.</param>
		/// <returns>Original decimal value.</returns>
		decimal DeObfuscate(decimal obfuscatedValue);

		/// <summary>
		/// Deobfuscate an double value.
		/// </summary>
		/// <param name="obfuscatedValue">Obfuscated double value to deobfuscate.</param>
		/// <returns>Original double value.</returns>
		double DeObfuscate(double obfuscatedValue);

		/// <summary>
		/// Deobfuscate a float value.
		/// </summary>
		/// <param name="obfuscatedValue">Obfuscated float value to deobfuscate.</param>
		/// <returns>Original float value.</returns>
		float DeObfuscate(float obfuscatedValue);

		/// <summary>
		/// Deobfuscate an integer value.
		/// </summary>
		/// <param name="obfuscatedValue">Obfuscated integer value to deobfuscate.</param>
		/// <returns>Original integer value.</returns>
		int DeObfuscate(int obfuscatedValue);

		/// <summary>
		/// Deobfuscate a long value.
		/// </summary>
		/// <param name="obfuscatedValue">Obfuscated long value to deobfuscate.</param>
		/// <returns>Original long value.</returns>
		long DeObfuscate(long obfuscatedValue);
	}
}
