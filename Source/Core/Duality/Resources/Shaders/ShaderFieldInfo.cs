using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;


namespace Duality.Resources
{
	/// <summary>
	/// Provides information about a shader variable.
	/// </summary>
	public class ShaderFieldInfo
	{
		private ShaderFieldScope scope;
		private ShaderFieldType type;
		private int arrayLength;
		private string name;
		private bool isPrivate;

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
		/// [GET] Returns whether the shader variable should be considered private.
		/// </summary>
		public bool IsPrivate
		{
			get { return this.isPrivate; }
		}

		public ShaderFieldInfo(string name, ShaderFieldType type, ShaderFieldScope scope, int arrayLength = 1)
		{
			this.name = name;
			this.type = type;
			this.scope = scope;
			this.arrayLength = arrayLength;
			this.isPrivate = string.IsNullOrEmpty(name) || name[0] == '_';
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
