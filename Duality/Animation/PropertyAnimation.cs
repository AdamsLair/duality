using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	public class PropertyAnimation<TObject,TValue> : Animation<TObject,TValue>, IPropertyAnimation where TObject : class
	{
		private	PropertyInfo	property	= null;
		public PropertyInfo Property
		{
			get { return this.property; }
		}
		public PropertyAnimation(TObject target, PropertyInfo property, AnimationTrack<TValue> track) : base(target, property.BuildPropertySetter<TObject,TValue>(), track)
		{
			if (target == null && !property.GetSetMethod().IsStatic)	throw new ArgumentNullException("target");
			this.property = property;
		}
	}

	public static class PropertyAnimation
	{
		public static PropertyAnimation<object,T> Create<T>(Expression<Func<T>> propertyExpression, AnimationTrack<T> track)
		{
			UnaryExpression convertExpr = propertyExpression.Body as UnaryExpression;
			MemberExpression memberExpr = propertyExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new PropertyAnimation<object,T>(
				null, 
				(PropertyInfo)memberExpr.Member, 
				track);
		}
		public static PropertyAnimation<object,T> Create<T>(PropertyInfo property, AnimationTrack<T> track)
		{
			return new PropertyAnimation<object,T>(
				null, 
				property, 
				track);
		}
		public static PropertyAnimation<T,U> Create<T,U>(T target, Expression<Func<U>> propertyExpression, AnimationTrack<U> track) where T : class
		{
			UnaryExpression convertExpr = propertyExpression.Body as UnaryExpression;
			MemberExpression memberExpr = propertyExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new PropertyAnimation<T,U>(
				target, 
				(PropertyInfo)memberExpr.Member, 
				track);
		}
		public static PropertyAnimation<T,U> Create<T,U>(T target, PropertyInfo property, AnimationTrack<U> track) where T : class
		{
			return new PropertyAnimation<T,U>(
				target, 
				property, 
				track);
		}
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
