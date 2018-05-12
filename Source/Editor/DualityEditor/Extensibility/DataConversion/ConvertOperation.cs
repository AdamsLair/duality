using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;

namespace Duality.Editor
{
	public class ConvertOperation
	{
		private struct ConvComplexityEntry
		{
			public DataConverter Converter;
			public int Complexity;

			public ConvComplexityEntry(DataConverter converter, int complexity)
			{
				this.Converter = converter;
				this.Complexity = complexity;
			}
		}
		private struct NameSuggestion
		{
			public object Target;
			public string Name;

			public NameSuggestion(object obj, string name)
			{
				this.Target = obj;
				this.Name = name;
			}
		}

		[Flags]
		public enum Operation
		{
			None		= 0x0,

			/// <summary>
			/// A simple conversion operation that does not affect any data.
			/// Example: Retrieving Texture from Material.
			/// </summary>
			Convert		= 0x1,
			/// <summary>
			/// A conversion that might create new resource data.
			/// Example: Creating Material from Texture.
			/// </summary>
			CreateRes	= 0x2,
			/// <summary>
			/// A conversion that might create new object data.
			/// Example: Construct a GameObject out of a set of Resources.
			/// </summary>
			CreateObj	= 0x4,

			All		= Convert | CreateRes | CreateObj
		}

		
		private static List<DataConverter> converters = new List<DataConverter>();
		
		internal static void Init()
		{
			foreach (TypeInfo genType in DualityEditorApp.GetAvailDualityEditorTypes(typeof(DataConverter)))
			{
				if (genType.IsAbstract) continue;
				DataConverter gen = genType.CreateInstanceOf() as DataConverter;
				if (gen != null) converters.Add(gen);
			}
		}
		internal static void Terminate()
		{
			converters.Clear();
		}
		private static IEnumerable<DataConverter> GetConverters(Type target)
		{
			return converters.Where(c => target.IsAssignableFrom(c.TargetType));
		}


		private	Operation		allowedOp	= Operation.All;
		private	ConversionData	data		= null;
		private	List<object>	result		= new List<object>();
		private	HashSet<object>	handledObj	= new HashSet<object>();
		// For "converter pathfinding":
		private	HashSet<DataConverter>	usedConverters	= new HashSet<DataConverter>();	// Flags "visited nodes" (DataConverters)
		private	HashSet<Type>			checkedTypes	= new HashSet<Type>();			// Flags "visited nodes" (CanPerform-Types)
		private	int						curComplexity	= 0;							// Measures "path length" (working variable)
		private	int						maxComplexity	= 0;							// Measures "path length" (final value)


		public ConversionData Data
		{
			get { return this.data; }
		}
		public IEnumerable<object> Result
		{
			get { return this.result; }
		}
		public Operation AllowedOperations
		{
			get { return this.allowedOp; }
		}


		public ConvertOperation(IDataObject data, Operation allowedOp)
		{
			allowedOp &= data.GetAllowedConvertOp();

			this.data = new ConversionData(data);
			this.allowedOp = allowedOp;
		}
		public ConvertOperation(IEnumerable<object> data, Operation allowedOp)
		{
			DataObject dataObj = new DataObject();
			foreach (object obj in data)
			{
				if (obj == null) continue;
				dataObj.SetData(obj.GetType(), obj);
			}
			this.data = new ConversionData(dataObj);
			this.allowedOp = allowedOp;
		}

		public void Reset()
		{
			this.handledObj.Clear();
			this.result.Clear();
			this.usedConverters.Clear();
			this.checkedTypes.Clear();
			this.curComplexity = 0;
			this.maxComplexity = 0;
		}
			
		public bool IsObjectHandled(object data)
		{
			if (data == null) return false;
			if (data is IContentRef) data = (data as IContentRef).Res;
			return this.handledObj.Contains(data);
		}
		public void MarkObjectHandled(object data)
		{
			if (data == null) return;
			if (data is IContentRef) data = (data as IContentRef).Res;
			this.handledObj.Add(data);
		}
		public void AddResult(object obj)
		{
			if (obj == null) return;
			if (this.result.Contains(obj)) return;
			this.result.Add(obj);
		}
		public void AddResult(IEnumerable<object> objEnum)
		{
			foreach (object obj in objEnum)
				this.AddResult(obj);
		}
		public void AddResult(params object[] objArray)
		{
			this.AddResult(objArray as IEnumerable<object>);
		}
		
		public void SuggestResultName(object obj, string name)
		{
			if (obj == null) return;
			if (name == null) return;
			if (name.Length == 0) return;
			this.AddResult(new NameSuggestion(obj, name));
		}
		public string TakeSuggestedResultName(object obj)
		{
			object nameSuggestion = this.result.FirstOrDefault(o => o is NameSuggestion && !this.IsObjectHandled(o) && ((NameSuggestion)o).Target == obj);
			if (nameSuggestion == null) return null;
			this.MarkObjectHandled(nameSuggestion);
			return ((NameSuggestion)nameSuggestion).Name;
		}

		public bool CanPerform<T>(Operation restrictTo = Operation.All)
		{
			return this.CanPerform(typeof(T), restrictTo);
		}
		public IEnumerable<T> Perform<T>(Operation restrictTo = Operation.All)
		{
			IEnumerable<object> result = this.Perform(typeof(T), restrictTo); 
			IEnumerable<T> castResult = result.OfType<T>();
			return castResult;
		}
		public bool CanPerform(Type target, Operation restrictTo = Operation.All)
		{
			Operation oldAllowedOp = this.allowedOp;
			this.allowedOp = oldAllowedOp & restrictTo;

			// Convert ContentRef requests to their respective Resource-requests
			target = ResTypeFromRefType(target);

			if (this.checkedTypes.Contains(target))
			{
				this.allowedOp = oldAllowedOp;
				return false;
			}
			this.curComplexity++;
			this.checkedTypes.Add(target);
			
			bool result = false;
			if (!result && this.data.GetDataPresent(target)) result = true;
			if (!result && this.data.GetDataPresent(target.MakeArrayType())) result = true;
			if (!result && this.data.ContainsContentRefs(target)) result = true;
			if (!result && this.data.ContainsComponents(target)) result = true;
			if (!result)
			{
				result = GetConverters(target).Any(s => !this.usedConverters.Contains(s) && s.CanConvertFrom(this));
			}

			if (result || this.allowedOp != oldAllowedOp) this.checkedTypes.Remove(target);
			this.maxComplexity = Math.Max(this.maxComplexity, this.curComplexity);
			this.curComplexity--;

			this.allowedOp = oldAllowedOp;
			return result;
		}
		public IEnumerable<object> Perform(Type target, Operation restrictTo = Operation.All)
		{
			Operation oldAllowedOp = this.allowedOp;
			this.allowedOp = oldAllowedOp & restrictTo;

			// Convert ContentRef requests to their respective Resource-requests
			Type originalType = target;
			target = ResTypeFromRefType(target);

			//Log.Editor.Write("Convert to {0}", target.Name);
			bool fittingDataFound = false;

			// Check if there already is fitting data available
			IEnumerable<object> fittingData = null;
			if (fittingData == null)
			{
				// Single object
				if (this.data.GetDataPresent(target)) fittingData = new[] { this.data.GetData(target) };
			}
			if (fittingData == null)
			{
				// Object array
				Type arrType = target.MakeArrayType();
				if (this.data.GetDataPresent(arrType)) fittingData = this.data.GetData(arrType) as IEnumerable<object>;
			}
			if (fittingData == null)
			{
				// ComponentRefs
				if (this.data.ContainsComponents(target)) fittingData = this.data.GetComponents(target);
			}
			if (fittingData == null)
			{
				// ContentRefs
				if (this.data.ContainsContentRefs(target)) fittingData = this.data.GetContentRefs(target).Res();
			}
			
			// If something fitting was found, directly add it to the operation results
			if (fittingData != null)
			{
				fittingDataFound = true;
				foreach (object obj in fittingData)
					this.AddResult(obj);
			}

			// No result yet? Search suitable converters
			if (!fittingDataFound)
			{
				var converterQuery = GetConverters(target);
				List<ConvComplexityEntry> converters = new List<ConvComplexityEntry>();
				foreach (var c in converterQuery)
				{
					this.maxComplexity = 0;
					if (this.usedConverters.Contains(c)) continue;
					if (!c.CanConvertFrom(this)) continue;
					converters.Add(new ConvComplexityEntry(c, this.maxComplexity));
				}

				// Perform conversion
				try
				{
					converters.StableSort((c1, c2) => (c2.Converter.Priority - c1.Converter.Priority) * 10000 + (c1.Complexity - c2.Complexity));
					foreach (var c in converters)
					{
						this.usedConverters.Add(c.Converter);
						bool handled = c.Converter.Convert(this);
						this.usedConverters.Remove(c.Converter);
						if (handled) break;
					}
				}
				// Since convert operations are often performed in dragdrop handlers (which internally catch all exceptions),
				// we should do some basic error logging in here to make sure users won't end up without explanation for
				// their operation not performing correctly or at all.
				catch (Exception e)
				{
					Log.Editor.WriteError(
						"There was an error trying to convert data to target type {0}: {1}", 
						Log.Type(target), 
						Log.Exception(e));
				}
			}

			IEnumerable<object> returnValue = this.result;

			// Convert back to Resource requests
			if (typeof(IContentRef).IsAssignableFrom(originalType))
				returnValue = result.OfType<Resource>().Select(r => r.GetContentRef());

			returnValue = returnValue ?? (IEnumerable<object>)Array.CreateInstance(originalType, 0);
			returnValue = returnValue.Where(originalType.IsInstanceOfType);

			this.allowedOp = oldAllowedOp;
			return returnValue;
		}

		private static Type ResTypeFromRefType(Type type)
		{
			if (typeof(IContentRef).IsAssignableFrom(type))
			{
				type = type.IsGenericType ? type.GetGenericArguments()[0] : typeof(Resource);
			}

			return type;
		}
	}
}
