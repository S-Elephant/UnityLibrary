#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Elephant.UnityLibrary.Extensions;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Geometry base class.
	/// </summary>
	[Serializable]
	public abstract class Geometry : ICloneable, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Contains all <see cref="Geometry"/>s that use this <see cref="Geometry"/> instance.
		/// For example a vertex may have a line as its parent.
		/// These parents are also used as observers.
		/// </summary>
		protected List<Geometry> Parents = new();

		/// <inheritdoc cref="Parents"/>
		public IReadOnlyList<Geometry> ParentsAsReadonly() => Parents.AsReadOnly();

		/// <summary>
		/// Delegate for handling the Recalculated event.
		/// </summary>
		/// <param name="sender">The <see cref="Geometry"/> object that raised the event.</param>
		public delegate void RecalculatedEventHandler(Geometry sender);

		/// <summary>
		/// Occurs after the geometry is recalculated.
		/// </summary>
		public event RecalculatedEventHandler OnRecalculated;

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
					foreach (Geometry parent in Parents)
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
		protected virtual void InvokeOnPropertyChanged([CallerMemberName] string propertyName = null)
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
				Parents.AddIfNotExists(parent);
		}

		/// <summary>
		/// Remove a specific parent.
		/// </summary>
		public virtual void RemoveParent(Geometry? parent)
		{
			if (parent != null)
				Parents.Remove(parent);
		}
	}
}
