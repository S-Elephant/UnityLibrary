using Elephant.UnityLibrary.Extensions;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// <see cref="ListExtensions"/> tests.
	/// </summary>
	public class ListExtensionsTests
	{
		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// throws an <see cref="ArgumentOutOfRangeException"/> if used on an empty enumerable.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void EmptyGetRandomAndRemoveThrows()
		{
			// Arrange.
			List<int> emptyEnumerable = new();

			// Act & Assert.
			Assert.Throws<ArgumentOutOfRangeException>(() => emptyEnumerable.GetRandomAndRemove());
		}

		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// throws an <see cref="NullReferenceException"/> if used on a null value.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void NullGetRandomAndRemoveThrows()
		{
			// Arrange.
			List<int>? emptyEnumerable = null;

			// Act & Assert.
			Assert.Throws<NullReferenceException>(() => emptyEnumerable!.GetRandomAndRemove());
		}

		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// must have removed exactly 1 element.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void GetRandomAndRemove_OneElementIsRemoved()
		{
			// Arrange.
			List<int> list = new() { 1, 2, -3 };

			// Act.
			int value = list.GetRandomAndRemove();

			// Assert.
			Assert.Equal(2, list.Count);
		}

		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// must return a valid element.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void GetRandomAndRemove_ValidElementIsReturned()
		{
			// Arrange.
			List<int> list = new() { 1, 2, -3 };

			// Act.
			int value = list.GetRandomAndRemove();

			// Assert.
			Assert.True(value == 1 || value == 2 || value == -3);
		}

		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// must have removed the last element.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void GetRandomAndRemove_LastElementIsRemoved()
		{
			// Arrange.
			List<int> list = new() { int.MaxValue };

			// Act.
			int value = list.GetRandomAndRemove();

			// Assert.
			Assert.Empty(list);
		}

		/// <summary>
		/// <see cref="ListExtensions.GetRandomAndRemove{T}(List{T})"/>
		/// must return the last element.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void GetRandomAndRemove_LastElementIsRetrieved()
		{
			// Arrange.
			List<int> list = new() { int.MaxValue };

			// Act.
			int value = list.GetRandomAndRemove();

			// Assert.
			Assert.Equal(int.MaxValue, value);
		}
	}
}
