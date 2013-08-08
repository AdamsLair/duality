using System;
using System.Linq;
using System.Drawing;

using Duality;
using Duality.Resources;

namespace DualityEditor.CorePluginInterface
{
	public enum PreviewSizeMode
	{
		FixedNone,
		FixedWidth,
		FixedHeight,
		FixedBoth
	}

	public static class PreviewProvider
	{
		public static Bitmap GetPreviewImage(object obj, int desiredWidth, int desiredHeight, PreviewSizeMode mode = PreviewSizeMode.FixedNone)
		{
			PreviewImageQuery query = new PreviewImageQuery(obj, desiredWidth, desiredHeight, mode);
			GetPreview(query);
			return query.Result;
		}
		public static Sound GetPreviewSound(object obj)
		{
			PreviewSoundQuery query = new PreviewSoundQuery(obj);
			GetPreview(query);
			return query.Result;
		}
		public static void GetPreview(IPreviewQuery query)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;
			if (query == null) return;
			
			//System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
			//w.Restart();

			var generators = (
				from g in CorePluginRegistry.GetPreviewGenerators()
				orderby query.SourceFits(g.ObjectType) descending, g.Priority descending
				select g).ToArray();

			foreach (IPreviewGenerator gen in generators)
			{
				if (!query.TransformSource(gen.ObjectType)) continue;

				gen.Perform(query);

				if (query.Result != null) break;
			}

			//Log.Editor.Write("Generating preview for {0} / {1} took {2:F} ms", query.OriginalSource, query.GetType().Name, w.Elapsed.TotalMilliseconds);
		}
	}



	public interface IPreviewQuery
	{
		object OriginalSource { get; }
		object Source { get; }
		object Result { get; }

		bool SourceFits(Type sourceType);
		bool TransformSource(Type sourceType);
	}
	public abstract class PreviewQuery<T> : IPreviewQuery where T : class
	{
		private		object	source	= null;
		protected	T		result	= null;

		private		object				sourceTransformed	= null;
		private		ConvertOperation	sourceConv			= null;
		
		public object OriginalSource
		{
			get { return this.source; }
		}
		public object Source
		{
			get { return this.sourceTransformed; }
		}
		public T Result
		{
			get { return this.result; }
			set { this.result = value; }
		}

		public PreviewQuery(object src)
		{
			this.source = src;
			this.sourceTransformed = this.source;
		}
		
		private void CreateConvert()
		{
			if (this.sourceConv != null) return;
			this.sourceConv = new ConvertOperation(new[] { this.source }, ConvertOperation.Operation.Convert);
		}
		bool IPreviewQuery.SourceFits(Type sourceType)
		{
			this.CreateConvert();
			return this.sourceConv.CanPerform(sourceType, ConvertOperation.Operation.None);
		}
		bool IPreviewQuery.TransformSource(Type sourceType)
		{
			this.CreateConvert();

			if (this.sourceConv.CanPerform(sourceType))
				this.sourceTransformed = this.sourceConv.Perform(sourceType).FirstOrDefault();
			else
				this.sourceTransformed = null;

			return this.sourceTransformed != null;
		}
		object IPreviewQuery.Result
		{
			get { return this.result; }
		}
	}
	public class PreviewImageQuery : PreviewQuery<Bitmap>
	{
		public int DesiredWidth { get; private set; }
		public int DesiredHeight { get; private set; }
		public PreviewSizeMode SizeMode { get; private set; }

		public PreviewImageQuery(object src, int desiredWidth, int desiredHeight, PreviewSizeMode mode) : base(src)
		{
			this.DesiredWidth = desiredWidth;
			this.DesiredHeight = desiredHeight;
			this.SizeMode = mode;
		}
	}
	public class PreviewSoundQuery : PreviewQuery<Sound>
	{
		public PreviewSoundQuery(object src) : base(src) {}
	}
	


	public interface IPreviewGenerator
	{
		int Priority { get; }
		Type ObjectType { get; }

		void Perform(IPreviewQuery settings);
	}
	public abstract class PreviewGenerator<T> : IPreviewGenerator
	{
		public virtual int Priority
		{
			get { return CorePluginRegistry.Priority_General; }
		}
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
