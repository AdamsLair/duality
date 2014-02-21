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
	public class PropertyAnimation<TObject,TValue> : Animation<TObject,TValue>, IPropertyAnimation where TObject : class
	{
		private	PropertyInfo	property	= null;
		/// <summary>
		/// [GET] The animated property.
		/// </summary>
		public PropertyInfo Property
		{
			get { return this.property; }
		}
		/// <summary>
		/// Creates a new property animation.
		/// </summary>
		/// <param name="target">The animated object.</param>
		/// <param name="field">The property that is set when updating the animated value.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		public PropertyAnimation(TObject target, PropertyInfo property, AnimationTrack<TValue> track) : base(target, property.BuildPropertySetter<TObject,TValue>(), track)
		{
			if (target == null && !property.GetSetMethod().IsStatic)	throw new ArgumentNullException("target");
			this.property = property;
		}
	}
	
	/// <summary>
	/// A static class that contains helper methods for easily creating new <see cref="PropertyAnimation{T,U}">property animations</see>.
	/// </summary>
	public static class PropertyAnimation
	{
		/// <summary>
		/// Creates a new <see cref="PropertyAnimation{T,U}">animation</see> of a static property.
		/// </summary>
		/// <typeparam name="T">Type of the animated value.</typeparam>
		/// <param name="propertyExpression">An lambda expression that references the static property using its method body, e.g. <code>() => SomeClass.MyProperty</code></param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static PropertyAnimation<object,T> Create<T>(Expression<Func<T>> propertyExpression, AnimationTrack<T> track)
		{
			UnaryExpression convertExpr = propertyExpression.Body as UnaryExpression;
			MemberExpression memberExpr = propertyExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new PropertyAnimation<object,T>(
				null, 
				(PropertyInfo)memberExpr.Member, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="PropertyAnimation{T,U}">animation</see> of a static property.
		/// </summary>
		/// <typeparam name="T">Type of the animated value.</typeparam>
		/// <param name="property">The animated static property.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static PropertyAnimation<object,T> Create<T>(PropertyInfo property, AnimationTrack<T> track)
		{
			return new PropertyAnimation<object,T>(
				null, 
				property, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="PropertyAnimation{T,U}">animation</see> of an instance property.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="propertyExpression">An lambda expression that references the static property using its method body, e.g. <code>() => someInstance.MyProperty</code></param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static PropertyAnimation<T,U> Create<T,U>(T target, Expression<Func<U>> propertyExpression, AnimationTrack<U> track) where T : class
		{
			UnaryExpression convertExpr = propertyExpression.Body as UnaryExpression;
			MemberExpression memberExpr = propertyExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new PropertyAnimation<T,U>(
				target, 
				(PropertyInfo)memberExpr.Member, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="PropertyAnimation{T,U}">animation</see> of an instance property.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="property">The animated instance property.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static PropertyAnimation<T,U> Create<T,U>(T target, PropertyInfo property, AnimationTrack<U> track) where T : class
		{
			return new PropertyAnimation<T,U>(
				target, 
				property, 
				track);
		}
		/// <summary>
		/// Creates a new <see cref="PropertyAnimation{T,U}">animation</see> of an instance property.
		/// </summary>
		/// <typeparam name="T">Type of the animated object.</typeparam>
		/// <typeparam name="U">Type of the animated value.</typeparam>
		/// <param name="target">The animated object.</param>
		/// <param name="propertyName">Name of the animated instance property.</fieldName>
		/// <param name="track">The animation track that is used as a data source.</param>
		/// <returns></returns>
		public static PropertyAnimation<T,U> Create<T,U>(T target, string propertyName, AnimationTrack<U> track) where T : class
		{
			Type targetType = (target != null) ? target.GetType() : typeof(T);
			return new PropertyAnimation<T,U>(
				target, 
				targetType.GetProperty(propertyName, ReflectionHelper.BindInstanceAll), 
				track);
		}
	}
}
