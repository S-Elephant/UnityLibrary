#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Geometry base class.
	/// </summary>
	[Serializable]
	public abstract class Geometry : ICloneable, INotifyPropertyChanged
	{
		/// <summary>
		/// Is called after a property is changed.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged = null;

		/// <summary>
		/// Contains all <see cref="Geometry"/>s that use this <see cref="Geometry"/> instance.
		/// For example a vertex may have a line as its parent.
		/// These parents are also used as observers.
		/// </summary>
		protected List<Geometry> _parents = new();

		/// <inheritdoc cref="_parents"/>
		public IReadOnlyList<Geometry> ParentsAsReadonly() => _parents.AsReadOnly();

		/// <summary>
		/// Delegate for handling the Recalculated event.
		/// </summary>
		/// <param name="sender">The <see cref="Geometry"/> object that raised the event.</param>
		public delegate void RecalculatedEventHandler(Geometry sender);

		/// <summary>
		/// Occurs after the geometry is recalculated.
		/// </summary>
		public event RecalculatedEventHandler? OnRecalculated = null;

		/// <inheritdoc cref="GeoSystems.GeometryType"/>
		public abstract GeometryType GeometryType { get; }

		private bool _isDirty = true;

		/// <summary>
		/// If true then recalculation(s) are required.
		/// By default all new <see cref="Geometry"/> is dirty.
		/// </summary>
		public bool IsDirty
		{
			get => _isDirty;
			protected set
			{
				if (_isDirty == value)
					return;

				_isDirty = value;
				if (_isDirty)
				{
					foreach (Geometry parent in _parents)
						parent.MarkAsDirty();
				}
			}
		}

		/// <summary>
		/// If true then no recalculation(s) are required.
		/// By default all new <see cref="Geometry"/> is dirty.
		/// </summary>
		public bool IsClean => !_isDirty;

		/// <summary>
		/// Marks this <see cref="Geometry"/> as dirty, meaning that some
		/// properties need to be updated before they can be used.
		/// </summary>
		public virtual void MarkAsDirty()
		{
			IsDirty = true;
		}

		/// <summary>
		/// Recalculate geometry values and sets <see cref="IsDirty"/> to <c>false</c>.
		/// </summary>
		public virtual void Recalculate()
		{
			IsDirty = false;
		}

		/// <summary>
		/// Invokes <see cref="OnRecalculated"/>.
		/// Call this method after <see cref="Recalculate"/> finished.
		/// </summary>
		protected virtual void InvokeOnRecalculated()
		{
			OnRecalculated?.Invoke(this);
		}

		/// <summary>
		/// Marks this and its parents as dirty and invokes <see cref="PropertyChanged"/>.
		/// </summary>
		protected virtual void InvokeOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			MarkAsDirty();

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <inheritdoc cref="ICloneable.Clone"/>
		public abstract object Clone();

		/// <summary>
		/// Add a specific parent.
		/// </summary>
		public virtual void AddParent(Geometry? parent)
		{
			if (parent != null)
				_parents.AddIfNotExists(parent);
		}

		/// <summary>
		/// Remove a specific parent.
		/// </summary>
		public virtual void RemoveParent(Geometry? parent)
		{
			if (parent != null)
				_parents.Remove(parent);
		}

		/// <summary>
		/// Retrieve a new list of all vertices in this geometry.
		/// </summary>
		public abstract List<GeometryVertex> AllVertices();

		/// <summary>
		/// Translate/Move this geometry.
		/// </summary>
		/// <param name="translation">Translation amount.</param>
		/// <param name="space">
		/// Space.World: Applies the <paramref name="translation"/> in world space, taking into account the global coordinate system.
		/// Space.Self: Applies the <paramref name="translation"/> in local space, relative to the geometry its own coordinate system
		/// and will do nothing if <paramref name="translation"/> is 0,0.
		/// </param>
		public abstract void Translate(Vector2 translation, Space space = Space.Self);
	}
}
