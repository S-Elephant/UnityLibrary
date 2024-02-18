using System;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Interfaces
{
	/// <summary>
	/// Vertex or point.
	/// </summary>
	public interface IVertex : ICloneable
	{
		/// <summary>
		/// Position.
		/// </summary>
		/// <remarks>Changing this to a different value will mark this <see cref="IVertex"/> as dirty.</remarks>
		Vector2 Position { get; set; }
	}
}
