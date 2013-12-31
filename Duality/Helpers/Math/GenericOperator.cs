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
			return SingleType<T>.Lerp(first, second, factor);
		}
		
		
		private static readonly Type[] SortedNumericPrimitives = new[]
		{
			typeof(double),
			typeof(float),
			typeof(long),
			typeof(ulong),
			typeof(int),
			typeof(uint),
			typeof(short),
			typeof(ushort),
			typeof(sbyte),
			typeof(byte),
			typeof(bool)
		};

		private static Type SelectIntermediateType<T,U>()
		{
			if (typeof(T) == typeof(U)) return typeof(T);

			bool firstInList = false;
			bool secondInList = false;
			Type favored = null;
			for (int i = 0; i < SortedNumericPrimitives.Length; i++)
			{
				if (SortedNumericPrimitives[i] == typeof(T))
				{
					if (favored == null) favored = typeof(T);
					firstInList = true;
				}
				if (SortedNumericPrimitives[i] == typeof(U))
				{
					if (favored == null) favored = typeof(U);
					secondInList = true;
				}
				if (firstInList && secondInList) return favored;
			}

			return typeof(T);
		}
		[System.Diagnostics.DebuggerNonUserCode] 
		private static Func<TParamA,TParamB,TParamC,TResult> CreateOperatorFunc<TParamA,TParamB,TParamC,TResult>(Func<Expression,Expression,Expression,Expression> mainExpressionConstruct, Type intermediateType = null, bool exceptionFallback = true)
		{
			try
			{
				ParameterExpression paramA = Expression.Parameter(typeof(TParamA));
				ParameterExpression paramB = Expression.Parameter(typeof(TParamB));
				ParameterExpression paramC = Expression.Parameter(typeof(TParamC));
				try
				{
					return Expression.Lambda<Func<TParamA,TParamB,TParamC,TResult>>(mainExpressionConstruct(paramA, paramB, paramC), paramA, paramB, paramC).Compile();
				}
				catch (InvalidOperationException)
				{
					Expression exprA;
					Expression exprB;
					Expression exprC;
					Expression exprReturn;

					if (typeof(TParamA) != intermediateType && intermediateType != null)
						exprA = Expression.Convert(paramA, intermediateType);
					else
						exprA = paramA;

					if (typeof(TParamB) != intermediateType && intermediateType != null)
						exprB = Expression.Convert(paramB, intermediateType);
					else
						exprB = paramB;

					if (typeof(TParamC) != intermediateType && intermediateType != null)
						exprC = Expression.Convert(paramC, intermediateType);
					else
						exprC = paramC;

					exprReturn = mainExpressionConstruct(exprA, exprB, exprC);

					if (exprReturn.Type != typeof(TResult) && intermediateType != null)
						exprReturn = Expression.ConvertChecked(exprReturn, typeof(TResult));

					return Expression.Lambda<Func<TParamA,TParamB,TParamC,TResult>>(exprReturn, paramA, paramB, paramC).Compile();
				}
			}
            catch (Exception e)
            {
				if (exceptionFallback)
					return delegate { throw new InvalidOperationException(e.Message); };
				else
					return null;
            }
		}
		[System.Diagnostics.DebuggerNonUserCode] 
		private static Func<TParamA,TParamB,TResult> CreateOperatorFunc<TParamA,TParamB,TResult>(Func<Expression,Expression,Expression> mainExpressionConstruct, Type intermediateType = null, bool exceptionFallback = true)
		{
			try
			{
				ParameterExpression paramA = Expression.Parameter(typeof(TParamA));
				ParameterExpression paramB = Expression.Parameter(typeof(TParamB));
				try
				{
					return Expression.Lambda<Func<TParamA,TParamB,TResult>>(mainExpressionConstruct(paramA, paramB), paramA, paramB).Compile();
				}
				catch (InvalidOperationException)
				{
					Expression exprA;
					Expression exprB;
					Expression exprReturn;

					if (typeof(TParamA) != intermediateType && intermediateType != null)
						exprA = Expression.Convert(paramA, intermediateType);
					else
						exprA = paramA;

					if (typeof(TParamB) != intermediateType && intermediateType != null)
						exprB = Expression.Convert(paramB, intermediateType);
					else
						exprB = paramB;

					exprReturn = mainExpressionConstruct(exprA, exprB);

					if (intermediateType != null)
						exprReturn = Expression.ConvertChecked(exprReturn, typeof(TResult));

					return Expression.Lambda<Func<TParamA,TParamB,TResult>>(exprReturn, paramA, paramB).Compile();
				}
			}
            catch (Exception e)
            {
				if (exceptionFallback)
					return delegate { throw new InvalidOperationException(e.Message); };
				else
					return null;
            }
		}
		[System.Diagnostics.DebuggerNonUserCode] 
		private static Func<TParam,TResult> CreateOperatorFunc<TParam,TResult>(Func<Expression,Expression> mainExpressionConstruct, Type intermediateType = null, bool exceptionFallback = true)
		{
			try
			{
				ParameterExpression param = Expression.Parameter(typeof(TParam));
				try
				{
					return Expression.Lambda<Func<TParam,TResult>>(mainExpressionConstruct(param), param).Compile();
				}
				catch (InvalidOperationException)
				{
					Expression expr;
					Expression exprReturn;

					if (typeof(TParam) != intermediateType && intermediateType != null)
						expr = Expression.ConvertChecked(param, intermediateType);
					else
						expr = param;

					exprReturn = mainExpressionConstruct(expr);

					if (intermediateType != null)
						exprReturn = Expression.ConvertChecked(exprReturn, typeof(TResult));

					return Expression.Lambda<Func<TParam,TResult>>(exprReturn, param).Compile();
				}
			}
            catch (Exception e)
            {
				if (exceptionFallback)
					return delegate { throw new InvalidOperationException(e.Message); };
				else
					return null;
            }
		}
		[System.Diagnostics.DebuggerNonUserCode] 
		private static Func<T,U> CreateNoOpFunc<T,U>()
		{
			try
			{
				ParameterExpression param = Expression.Parameter(typeof(T));
				return Expression.Lambda<Func<T,U>>(param, param).Compile();
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
			public static readonly Func<T,T,float,T> Lerp;

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
				Negate				= CreateOperatorFunc<T,T>(Expression.NegateChecked);
				Abs					= CreateOperatorFunc<T,T>(body => Expression.Condition(Expression.LessThan(body, Expression.Constant(default(T), typeof(T))), Expression.NegateChecked(body), body));

				Or					= CreateOperatorFunc<T,T,T>(Expression.Or);
				And					= CreateOperatorFunc<T,T,T>(Expression.And);
				Xor					= CreateOperatorFunc<T,T,T>(Expression.ExclusiveOr);
				Not					= CreateOperatorFunc<T,T>(Expression.Not);

				Equal				= CreateOperatorFunc<T,T,bool>(Expression.Equal);
				GreaterThan			= CreateOperatorFunc<T,T,bool>(Expression.GreaterThan);
				GreaterThanOrEqual	= CreateOperatorFunc<T,T,bool>(Expression.GreaterThanOrEqual);
				LessThan			= CreateOperatorFunc<T,T,bool>(Expression.LessThan);
				LessThanOrEqual		= CreateOperatorFunc<T,T,bool>(Expression.LessThanOrEqual);
				
				{
					Type intermediate = SelectIntermediateType<T,float>();
					Func<T,T,float,T> temp;

					// Try to create a Lerp term without casting the scale factor
					temp = CreateOperatorFunc<T,T,float,T>((left, right, factor) => 
						Expression.AddChecked(
							Expression.MultiplyChecked(left, Expression.SubtractChecked(Expression.Constant(1.0f), factor)), 
							Expression.MultiplyChecked(right, factor)
						),
						intermediate,
						false);

					// Doesn't work? Try with casting the scale factor to the intermediate Type then.
					if (temp == null)
					{
						temp = CreateOperatorFunc<T,T,float,T>((left, right, factor) => 
							Expression.AddChecked(
								Expression.MultiplyChecked(left, Expression.SubtractChecked(Expression.ConvertChecked(Expression.Constant(1.0f), intermediate), factor)), 
								Expression.MultiplyChecked(right, factor)
							),
							intermediate);
					}

					// Assign Lerp method;
					Lerp = temp;
				}
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
				Type intermediateType = SelectIntermediateType<T,U>();

				Add			= CreateOperatorFunc<T,U,T>(Expression.AddChecked, intermediateType);
				Subtract	= CreateOperatorFunc<T,U,T>(Expression.SubtractChecked, intermediateType);
				Multiply	= CreateOperatorFunc<T,U,T>(Expression.MultiplyChecked, intermediateType);
				Divide		= CreateOperatorFunc<T,U,T>(Expression.Divide, intermediateType);
				Convert		= (typeof(T) != typeof(U)) ? CreateOperatorFunc<T,U>(body => Expression.ConvertChecked(body, typeof(U))) : CreateNoOpFunc<T,U>();
			}
		}
	}
}
