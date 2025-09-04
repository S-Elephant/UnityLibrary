using Elephant.UnityLibrary.Extensions;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// <see cref="Elephant.UnityLibrary.Extensions.EnumerableExtensions"/> tests.
	/// </summary>
	public class EnumerableExtensionsTests
	{
		/// <summary>
		/// <see cref="Elephant.UnityLibrary.Extensions.EnumerableExtensions.GetRandom{T}(IEnumerable{T})"/>
		/// throws an <see cref="InvalidOperationException"/> if used on an empty enumerable.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void EmptyGetRandomThrows()
		{
			// Arrange.
			IEnumerable<int> emptyEnumerable = Enumerable.Empty<int>();

			// Act & Assert.
			Assert.Throws<InvalidOperationException>(() => emptyEnumerable.GetRandom());
		}

		/// <summary>
		/// <see cref="Elephant.UnityLibrary.Extensions.EnumerableExtensions.GetRandom{T}(IEnumerable{T})"/>
		/// throws an <see cref="InvalidOperationException"/> if used on a null value.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void NullGetRandomThrows()
		{
			// Arrange.
			IEnumerable<int>? emptyEnumerable = null;

			// Act & Assert.
			Assert.Throws<ArgumentNullException>(() => emptyEnumerable!.GetRandom());
		}
	}
}
