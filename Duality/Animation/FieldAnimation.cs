using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	public class FieldAnimation<TObject,TValue> : Animation<TObject,TValue>, IFieldAnimation where TObject : class
	{
		private	FieldInfo	field	= null;
		public FieldInfo Field
		{
			get { return this.field; }
		}
		public FieldAnimation(TObject target, FieldInfo field, AnimationTrack<TValue> track) : base(target, field.BuildFieldSetter<TObject,TValue>(), track)
		{
			if (target == null && !field.IsStatic)	throw new ArgumentNullException("target");
			this.field = field;
		}
	}

	public static class FieldAnimation
	{
		public static FieldAnimation<object,T> Create<T>(Expression<Func<T>> fieldExpression, AnimationTrack<T> track)
		{
			UnaryExpression convertExpr = fieldExpression.Body as UnaryExpression;
			MemberExpression memberExpr = fieldExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new FieldAnimation<object,T>(
				null, 
				(FieldInfo)memberExpr.Member, 
				track);
		}
		public static FieldAnimation<object,T> Create<T>(FieldInfo field, AnimationTrack<T> track)
		{
			return new FieldAnimation<object,T>(
				null, 
				field, 
				track);
		}
		public static FieldAnimation<T,U> Create<T,U>(T target, Expression<Func<U>> fieldExpression, AnimationTrack<U> track) where T : class
		{
			UnaryExpression convertExpr = fieldExpression.Body as UnaryExpression;
			MemberExpression memberExpr = fieldExpression.Body as MemberExpression ?? convertExpr.Operand as MemberExpression;
			return new FieldAnimation<T,U>(
				target, 
				(FieldInfo)memberExpr.Member, 
				track);
		}
		public static FieldAnimation<T,U> Create<T,U>(T target, FieldInfo property, AnimationTrack<U> track) where T : class
		{
			return new FieldAnimation<T,U>(
				target, 
				property, 
				track);
		}
		public static FieldAnimation<T,U> Create<T,U>(T target, string propertyName, AnimationTrack<U> track) where T : class
		{
			Type targetType = (target != null) ? target.GetType() : typeof(T);
			return new FieldAnimation<T,U>(
				target, 
				targetType.GetField(propertyName, ReflectionHelper.BindInstanceAll), 
				track);
		}
	}
}
