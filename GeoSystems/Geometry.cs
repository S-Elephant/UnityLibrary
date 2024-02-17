using System;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Geometry base class.
	/// </summary>
	public abstract class Geometry : ICloneable
	{
		/// <summary>
		/// If true then recalculation(s) are required.
		/// By default all new <see cref="Geometry"/> is dirty.
		/// </summary>
		public bool IsDirty { get; protected set; } = true;

		/// <summary>
		/// Recalculate geometry values and sets <see cref="IsDirty"/> to <c>false</c>.
		/// </summary>
		public virtual void Recalculate()
		{
			IsDirty = false;
		}

		/// <inheritdoc cref="ICloneable.Clone"/>
		public abstract object Clone();
	}
}
