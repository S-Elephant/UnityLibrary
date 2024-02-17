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
		Vector2 Position { get; set; }
	}
}
