using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	/// <summary>
	/// Represents the <see cref="Animation{T,U}">animation</see> of an objects field.
	/// </summary>
	/// <typeparam name="TObject">Type of the animated object.</typeparam>
	/// <typeparam name="TValue">Type of the animated value.</typeparam>
	public class FieldAnimation<TObject,TValue> : Animation<TObject,TValue>, IFieldAnimation where TObject : class
	{
		private	FieldInfo	field	= null;
		/// <summary>
		/// [GET] The animated field.
		/// </summary>
		public FieldInfo Field
		{
			get { return this.field; }
		}
		/// <summary>
		/// Creates a new field animation.
		/// </summary>
		/// <param name="target">The animated object.</param>
		/// <param name="field">The field that is set when updating the animated value.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		public FieldAnimation(TObject target, FieldInfo field, AnimationTrack<TValue> track) : base(target, field.BuildFieldSetter<TObject,TValue>(), track)
		{
			if (target == null && !field.IsStatic)	throw new ArgumentNullException("target");
			this.field = field;
		}
	}

	/// <summary>
	/// A static class that contains helper methods for easily creating new <see cref="FieldAnimation{T,U}">field animations</see>.
	/// </summary>
	public static class FieldAnimation
	{
		/// <summary>
		/// Creates a new <see cref="FieldAnimation{T,U}">animation</see> of a static field.
		/// </summary>
		/// <typeparam name="T">Type of the animated value.</typeparam>
		/// <param name="fieldExpression">An lambda expression that references the static field using its method body, e.g. <code>() => SomeClass.MyField</code></param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static FieldAnimation<object,T> Create<T>(Expression<Func<T>> fieldExpression, AnimationTrack<T> track)
		{
			UnaryExpression convertExpr = fieldExpression.Body as UnaryExpression;
			MemberExpression memberExpr = fieldExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new FieldAnimation<object,T>(
				null, 
				(FieldInfo)memberExpr.Member, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="FieldAnimation{T,U}">animation</see> of a static field.
		/// </summary>
		/// <typeparam name="T">Type of the animated value.</typeparam>
		/// <param name="field">The animated static field.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static FieldAnimation<object,T> Create<T>(FieldInfo field, AnimationTrack<T> track)
		{
			return new FieldAnimation<object,T>(
				null, 
				field, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="FieldAnimation{T,U}">animation</see> of an instance field.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="fieldExpression">An lambda expression that references the static field using its method body, e.g. <code>() => someInstance.MyField</code></param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static FieldAnimation<T,U> Create<T,U>(T target, Expression<Func<U>> fieldExpression, AnimationTrack<U> track) where T : class
		{
			UnaryExpression convertExpr = fieldExpression.Body as UnaryExpression;
			MemberExpression memberExpr = fieldExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new FieldAnimation<T,U>(
				target, 
				(FieldInfo)memberExpr.Member, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="FieldAnimation{T,U}">animation</see> of an instance field.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="field">The animated instance field.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static FieldAnimation<T,U> Create<T,U>(T target, FieldInfo property, AnimationTrack<U> track) where T : class
		{
			return new FieldAnimation<T,U>(
				target, 
				property, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="FieldAnimation{T,U}">animation</see> of an instance field.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="fieldName">Name of the animated instance field.</fieldName>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static FieldAnimation<T,U> Create<T,U>(T target, string fieldName, AnimationTrack<U> track) where T : class
		{
			Type targetType = (target != null) ? target.GetType() : typeof(T);
			return new FieldAnimation<T,U>(
				target, 
				targetType.GetField(fieldName, ReflectionHelper.BindInstanceAll), 
				track);
		}
	}
}
