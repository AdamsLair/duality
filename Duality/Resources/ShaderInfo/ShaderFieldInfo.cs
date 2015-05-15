using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;


namespace Duality.Resources
{
	/// <summary>
	/// Provides information about a <see cref="AbstractShader">shader</see> variable.
	/// </summary>
	public struct ShaderFieldInfo
	{
		/// <summary>
		/// The default variable name for a materials main texture.
		/// </summary>
		public const string VarName_MainTex = "mainTex";
		
		private ShaderFieldScope scope;
		private ShaderFieldType type;
		private int arrayLength;
		private string name;
		private int handle;

		/// <summary>
		/// [GET] The <see cref="ShaderFieldScope">scope</see> of the variable
		/// </summary>
		public ShaderFieldScope Scope
		{
			get { return this.scope; }
		}
		/// <summary>
		/// [GET] The <see cref="ShaderFieldType">type</see> of the variable
		/// </summary>
		public ShaderFieldType Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// [GET] If the variable is an array, this is its length. Arrays
		/// are only supported for <see cref="ShaderFieldType.Int"/> and
		/// <see cref="ShaderFieldType.Float"/>.
		/// </summary>
		public int ArrayLength
		{
			get { return this.arrayLength; }
		}
		/// <summary>
		/// [GET] The name of the variable, as declared in the shader.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// [GET] Native location handle of the variable, which can be used to set its value.
		/// </summary>
		public int Handle
		{
			get { return this.handle;}
		}
		/// <summary>
		/// [GET] Returns whether the shader variable should be considered private.
		/// </summary>
		public bool IsPrivate
		{
			get { return string.IsNullOrEmpty(this.Name) || this.Name[0] == '_'; }
		}

		public ShaderFieldInfo(string name, ShaderFieldType type, ShaderFieldScope scope, int arrayLength = 1, int handle = -1)
		{
			this.name = name;
			this.type = type;
			this.scope = scope;
			this.arrayLength = arrayLength;
			this.handle = handle;
		}

		public ShaderFieldInfo WithHandle(int handle)
		{
			return new ShaderFieldInfo(this.name, this.type, this.scope, this.arrayLength, handle);
		}

		public override string ToString()
		{
			return string.Format("{1} {0}{2}", 
				this.name, 
				this.type, 
				this.arrayLength > 1 ? string.Format("[{0}]", this.arrayLength) : "");
		}
	}
}
