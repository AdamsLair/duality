using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdamsLair.PropertyGrid
{
	public partial class DemoForm : Form
	{
		#region Some Test / Demo classes
		[Flags]
		private enum FlaggedEnumTest : uint
		{
			One	= 0x1,
			Two	= 0x2,
			Three = 0x4,

			OneAndThree = One | Three,
			None = 0x0,
			All = One | Two | Three
		}
		private enum EnumTest
		{
			One,
			Two,
			Three
		}
		private class Test
		{
			private int i;
			private int i2;
			private float f;
			private byte b;
			private int[] i3 = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
			private string t;
			private Test2 subclass;
			public List<string> stringListField;
			public FlaggedEnumTest enumField1;
			public EnumTest enumField2;
			private System.Diagnostics.Stopwatch w = System.Diagnostics.Stopwatch.StartNew();

			public int IPropWithAVeryLongName
			{
				get { return this.i; }
				set { this.i = value; }
			}
			public int SomeInt
			{
				get { return this.i2; }
				set { this.i2 = value * 2; }
			}
			public float SomeFloat
			{
				get { return this.f; }
				set { this.f = value; }
			}
			public byte SomeByte
			{
				get { return this.b; }
				set { this.b = value; }
			}
			public int[] SomeIntArray
			{
				get { return this.i3; }
			}
			public string SomeString
			{
				get { return this.t; }
				set { this.t = value; }
			}
			public string SomeString2
			{
				get { return this.t; }
				set { this.t = value; }
			}
			public Test2 Subclass
			{
				get { return this.subclass; }
				set { this.subclass = value; }
			}
			public bool BoolOne { get; set; }
			public bool BoolTwo { get; set; }
			public List<Test2> StructList { get; set; }
			public Dictionary<string,int> SomeDict { get; set; }
			public Dictionary<string,List<int>> SomeDict2 { get; set; }
			public TimeSpan ElapsedTime
			{
				get { return this.w.Elapsed; }
			}
			public string ElapsedTimeString
			{
				get { return this.w.Elapsed.ToString(); }
			}
			public long ElapsedTicks
			{
				get { return this.w.ElapsedTicks; }
			}
			public long ElapsedMs
			{
				get { return this.w.ElapsedMilliseconds; }
			}
			public double ElapsedMsHighPrecision
			{
				get { return this.w.Elapsed.TotalMilliseconds; }
			}
		}
		private struct Test2
		{
			private int yoink;
			public bool testBool;

			public int Yoink
			{
				get { return this.yoink; }
				set { this.yoink = value; }
			}

			public Test2(int val)
			{
				this.yoink = val;
				this.testBool = true;
			}

			public override string ToString()
			{
				return "Yoink: " + this.yoink;
			}
		}
		#endregion

		public DemoForm()
		{
			this.InitializeComponent();

			// Generate some test / demo objects
			Test testObj = new Test();
			testObj.IPropWithAVeryLongName = 42;
			testObj.SomeString = "Blubdiwupp";
			testObj.SomeFloat = (float)Math.PI;
			testObj.SomeByte = 128;
			testObj.Subclass = new Test2(42);
			testObj.stringListField = new List<string>() { "hallo", "welt" };
			Test testObj2 = new Test();
			testObj2.IPropWithAVeryLongName = 447;
			testObj2.SomeString = "Kratatazong";
			testObj2.Subclass = new Test2();
			testObj2.enumField2 = EnumTest.Three;

			// Case A: Singleselect
			this.propertyGrid1.SelectObject(testObj);
			// Case B: Multiselect
			//this.propertyGrid1.SelectObjects(new object[] { testObj, testObj2 });
			// Case C: Select this very form
			//this.propertyGrid1.SelectObject(this);
		}

		private void radioEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioEnabled.Checked)
			{
				this.propertyGrid1.Enabled = true;
				this.propertyGrid1.ReadOnly = false;
			}
		}
		private void radioReadOnly_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioReadOnly.Checked)
			{
				this.propertyGrid1.Enabled = true;
				this.propertyGrid1.ReadOnly = true;
			}
		}
		private void radioDisabled_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioDisabled.Checked)
			{
				this.propertyGrid1.Enabled = false;
				this.propertyGrid1.ReadOnly = false;
			}
		}
		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			this.propertyGrid1.UpdateFromObjects();
		}
		private void checkBoxNonPublic_CheckedChanged(object sender, EventArgs e)
		{
			this.propertyGrid1.ShowNonPublic = this.checkBoxNonPublic.Checked;
		}
	}
}
