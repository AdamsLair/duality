using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// Although not entirely equal, Generic Operators in Duality are heavily inspired by the ones implemented in 
// MiscUtil by Jon Skeet, see here: http://www.yoda.arachsys.com/csharp/miscutil/usage/genericoperators.html

namespace Duality
{
	/// <summary>
	/// Provides math operations for generic types that are dynamically resolved on their first usage.
	/// </summary>
	public static class GenericOperator
	{
		/// <summary>
		/// Adds two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Add<T,U>(T first, U second)
		{
			return DualType<T,U>.Add(first, second);
		}
		/// <summary>
		/// Subtracts two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Subtract<T,U>(T first, U second)
		{
			return DualType<T,U>.Subtract(first, second);
		}
		/// <summary>
		/// Multiplies two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Multiply<T,U>(T first, U second)
		{
			return DualType<T,U>.Multiply(first, second);
		}
		/// <summary>
		/// Divides two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Divide<T,U>(T first, U second)
		{
			return DualType<T,U>.Divide(first, second);
		}
		/// <summary>
		/// Calculates the modulo of a generic value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Modulo<T>(T first, T second)
		{
			return SingleType<T>.Modulo(first, second);
		}
		/// <summary>
		/// Negates a generic value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Negate<T>(T value)
		{
			return SingleType<T>.Negate(value);
		}
		/// <summary>
		/// Calculates the absolute (non-negative) value of a generic value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Abs<T>(T value)
		{
			return SingleType<T>.Abs(value);
		}

		/// <summary>
		/// Performs a bitwise OR on two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Or<T>(T first, T second)
		{
			return SingleType<T>.Or(first, second);
		}
		/// <summary>
		/// Performs a bitwise AND on two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T And<T>(T first, T second)
		{
			return SingleType<T>.And(first, second);
		}
		/// <summary>
		/// Performs a bitwise exclusive OR on two generic values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Xor<T>(T first, T second)
		{
			return SingleType<T>.Xor(first, second);
		}
		/// <summary>
		/// Performs a bitwise NOT on a generic value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static T Not<T>(T value)
		{
			return SingleType<T>.Not(value);
		}
		
		/// <summary>
		/// Determines whether two generic values are equal.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool Equal<T>(T first, T second)
		{
			return SingleType<T>.Equal(first, second);
		}
		/// <summary>
		/// Determines whether one generic value is greater than the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool GreaterThan<T>(T first, T second)
		{
			return SingleType<T>.GreaterThan(first, second);
		}
		/// <summary>
		/// Determines whether one generic value is greater than or equal to the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool GreaterThanOrEqual<T>(T first, T second)
		{
			return SingleType<T>.GreaterThanOrEqual(first, second);
		}
		/// <summary>
		/// Determines whether one generic value is less than the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool LessThan<T>(T first, T second)
		{
			return SingleType<T>.LessThan(first, second);
		}
		/// <summary>
		/// Determines whether one generic value is less than or equal to the other.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool LessThanOrEqual<T>(T first, T second)
		{
			return SingleType<T>.LessThanOrEqual(first, second);
		}

		/// <summary>
		/// Converts a generic value to a certain type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static U Convert<T,U>(T value)
		{
			return DualType<T,U>.Convert(value);
		}
		
		/// <summary>
		/// Performs a linear interpolation on a two generic values using a blend factor.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <param name="factor"></param>
		/// <returns></returns>
		public static T Lerp<T>(T first, T second, float factor)
		{
			return 
				DualType<T,T>.Add(
					DualType<T,float>.Multiply(first, 1.0f - factor), 
					DualType<T,float>.Multiply(second, factor)
				);
		}
		

		private static Func<T,U,V> CreateOperatorFunc<T,U,V>(Func<Expression,Expression,Expression> mainExpressionConstruct)
		{
			try
			{
				ParameterExpression paramLeft = Expression.Parameter(typeof(T));
				ParameterExpression paramRight = Expression.Parameter(typeof(U));
				try
				{
					return Expression.Lambda<Func<T,U,V>>(mainExpressionConstruct(paramLeft, paramRight), paramLeft, paramRight).Compile();
				}
				catch (InvalidOperationException)
				{
					Expression exprLeft;
					Expression exprRight;

					if (typeof(T) == typeof(V))
						exprLeft = paramLeft;
					else
						exprLeft = Expression.Convert(paramLeft, typeof(V));

					if (typeof(U) == typeof(V))
						exprRight = paramRight;
					else
						exprRight = Expression.Convert(paramRight, typeof(V));

					return Expression.Lambda<Func<T,U,V>>(mainExpressionConstruct(exprLeft, exprRight), paramLeft, paramRight).Compile();
				}
			}
            catch (Exception e)
            {
                return delegate { throw new InvalidOperationException(e.Message); };
            }
		}
		private static Func<T,U> CreateOperatorFunc<T,U>(Func<Expression,Expression> mainExpressionConstruct)
		{
			try
			{
				ParameterExpression param = Expression.Parameter(typeof(T));
				try
				{
					return Expression.Lambda<Func<T,U>>(mainExpressionConstruct(param), param).Compile();
				}
				catch (InvalidOperationException)
				{
					Expression expr;

					if (typeof(T) == typeof(U))
						expr = param;
					else
						expr = Expression.Convert(param, typeof(U));

					return Expression.Lambda<Func<T,U>>(mainExpressionConstruct(expr), param).Compile();
				}
			}
            catch (Exception e)
            {
                return delegate { throw new InvalidOperationException(e.Message); };
            }
		}
		private static class SingleType<T>
		{
			public static readonly Func<T,T,T> Modulo;
			public static readonly Func<T,T> Negate;
			public static readonly Func<T,T> Abs;

			public static readonly Func<T,T,T> Or;
			public static readonly Func<T,T,T> And;
			public static readonly Func<T,T,T> Xor;
			public static readonly Func<T,T> Not;

			public static readonly Func<T,T,bool> Equal;
			public static readonly Func<T,T,bool> GreaterThan;
			public static readonly Func<T,T,bool> GreaterThanOrEqual;
			public static readonly Func<T,T,bool> LessThan;
			public static readonly Func<T,T,bool> LessThanOrEqual;

			[System.Diagnostics.DebuggerNonUserCode] 
			static SingleType()
			{
				Modulo				= CreateOperatorFunc<T,T,T>(Expression.Modulo);
				Negate				= CreateOperatorFunc<T,T>(Expression.Negate);
				Abs					= CreateOperatorFunc<T,T>(body => Expression.Condition(Expression.LessThan(body, Expression.Constant(default(T), typeof(T))), Expression.Negate(body), body));

				Or					= CreateOperatorFunc<T,T,T>(Expression.Or);
				And					= CreateOperatorFunc<T,T,T>(Expression.And);
				Xor					= CreateOperatorFunc<T,T,T>(Expression.ExclusiveOr);
				Not					= CreateOperatorFunc<T,T>(Expression.Not);

				Equal				= CreateOperatorFunc<T,T,bool>(Expression.Equal);
				GreaterThan			= CreateOperatorFunc<T,T,bool>(Expression.GreaterThan);
				GreaterThanOrEqual	= CreateOperatorFunc<T,T,bool>(Expression.GreaterThanOrEqual);
				LessThan			= CreateOperatorFunc<T,T,bool>(Expression.LessThan);
				LessThanOrEqual		= CreateOperatorFunc<T,T,bool>(Expression.LessThanOrEqual);
			}
		}
		private static class DualType<T,U>
		{
			public static readonly Func<T,U,T> Add;
			public static readonly Func<T,U,T> Subtract;
			public static readonly Func<T,U,T> Multiply;
			public static readonly Func<T,U,T> Divide;
			public static readonly Func<T,U> Convert;

			[System.Diagnostics.DebuggerNonUserCode] 
			static DualType()
			{
				Add			= CreateOperatorFunc<T,U,T>(Expression.Add);
				Subtract	= CreateOperatorFunc<T,U,T>(Expression.Subtract);
				Multiply	= CreateOperatorFunc<T,U,T>(Expression.Multiply);
				Divide		= CreateOperatorFunc<T,U,T>(Expression.Divide);
				Convert		= CreateOperatorFunc<T,U>(body => Expression.Convert(body, typeof(U)));
			}
		}
	}
}
