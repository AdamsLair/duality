using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// A <see cref="IPreviewGenerator"/> that provides previews
	/// for objects of type T
	/// </summary>
	/// <typeparam name="T">
	/// The type of object that this <see cref="IPreviewGenerator"/>
	/// can generate previews for
	/// </typeparam>
	public abstract class PreviewGenerator<T> : IPreviewGenerator
	{
		public const int PriorityNone			= 0;
		public const int PriorityGeneral		= 20;
		public const int PrioritySpecialized	= 50;
		public const int PriorityOverride		= 100;

		/// <summary>
		/// The priority to assign to this generator. Generators
		/// with higher priorty will be checked first and the first
		/// preview successfully created will be used.
		/// Defaults to <see cref="PriorityGeneral"/>
		/// </summary>
		public virtual int Priority
		{
			get { return PriorityGeneral; }
		}
		/// <summary>
		/// The type of object that this generator can provide previews for.
		/// </summary>
		public Type ObjectType
		{
			get { return typeof(T); }
		}

		public virtual void Perform(T obj, PreviewImageQuery query) {}
		public virtual void Perform(T obj, PreviewSoundQuery query) {}

		void IPreviewGenerator.Perform(IPreviewQuery query)
		{
			PreviewImageQuery imgQuery = query as PreviewImageQuery;
			if (imgQuery != null)
			{
				this.Perform((T)query.Source, imgQuery);
				return;
			}
			PreviewSoundQuery sndQuery = query as PreviewSoundQuery;
			if (sndQuery != null)
			{
				this.Perform((T)query.Source, sndQuery);
				return;
			}
		}
	}
}
