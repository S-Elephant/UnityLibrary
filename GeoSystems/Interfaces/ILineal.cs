using System;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Interfaces
{
	/// <summary>
	/// Interface to identify all Geometry subclasses that have a dimension.
	/// </summary>
	public interface ILineal : ICloneable
	{
		/// <summary>
		/// AABB.
		/// </summary>
		Rect Aabb { get; }
	}
}
